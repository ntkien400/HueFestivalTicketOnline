using AutoMapper;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HueFestivalTicketOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEmail _sendEmail;
        private readonly ISendSms _sendSms;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, ISendEmail sendEmail, ISendSms sendSms, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sendEmail = sendEmail;
            _sendSms = sendSms;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Account>> GetAllAccount()
        {
            var listAcc = await _unitOfWork.Account.GetAllAsync();
            return Ok(listAcc);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Account>> Get(string id)
        {
            var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Id == id);
            if (acc != null)
            {
                return Ok(acc);
            }
            return BadRequest("Account is not exists");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var registerResult = await _unitOfWork.Account.Register(registerDto);
            if (registerResult.isExistEmail == true)
            {
                return BadRequest("Email is already used");
            }
            if (registerResult.isExistUser == true)
            {
                return BadRequest("Username is already used");
            }
            if (registerResult.validEmail == false)
            {
                return BadRequest("Email invalid");
            }
            if (registerResult.validPhone == false)
            {
                return BadRequest("Phone invalid");
            }
            if (registerResult.CreateUserResult == false)
            {
                return BadRequest(registerResult.errors);
            }
            return Ok("Account create successfully");

        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var loginResult = await _unitOfWork.Account.Login(loginDto);
            if (loginResult.CheckUserName == false)
            {
                return BadRequest("Username is wrong");
            }
            if (loginResult.CheckPassword == false)
            {
                return BadRequest("Password is wrong");
            }
            return Ok(loginResult.RefreshTokenDTO);
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string emailOrPhone)
        {
            var validEmail = new EmailAddressAttribute().IsValid(emailOrPhone);
            var validPhone = new PhoneAttribute().IsValid(emailOrPhone);
            if (validEmail)
            {
                var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Email == emailOrPhone);
                if (acc != null)
                {
                    acc.PasswordOTP = _sendSms.GenerateOTPCode(6);
                    acc.PasswordOTPExpried = DateTime.Now.AddMinutes(1);
                    _unitOfWork.Account.Update(acc);
                    await _unitOfWork.SaveAsync();
                    await _sendEmail.SendForgotPasswordEmailAsync(emailOrPhone, acc.PasswordOTP);
                    return Ok("OTP code sent to your mail");
                }
            }
            if (validPhone)
            {
                var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.PhoneNumber == emailOrPhone);
                if (acc != null)
                {
                    acc.PasswordOTP = _sendSms.GenerateOTPCode(6);
                    acc.PasswordOTPExpried = DateTime.Now.AddMinutes(1);
                    _unitOfWork.Account.Update(acc);
                    await _unitOfWork.SaveAsync();
                    _sendSms.SendOtpSms(emailOrPhone, acc.PasswordOTP);
                    return Ok("OTP code sent to your phone");
                }
            }
            return BadRequest("Email or Phone invalid");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto request)
        {
            var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.PasswordOTP == request.OTPCode);
            if (acc == null || acc.PasswordOTPExpried < DateTime.Now)
            {
                return BadRequest("Invalid OTP");
            }
            if (request.Password == request.ConfirmPassword)
            {
                await _unitOfWork.Account.ChangePassword(acc, request.Password);
                await _unitOfWork.SaveAsync();
                return Ok("Change password successfully");
            }
            return BadRequest("Confirm password is not same with password");


        }

        [HttpPut("change-password")]
        [Authorize(Roles = StaticUserRole.USER +","+ StaticUserRole.ADMIN)]
        public async Task<ActionResult> ChangePassword(string id, [FromBody] ChangeAccountPass accountPass)
        {
            var account = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Id == id);
            if(account != null)
            {
                if (accountPass.OldPassword != null && accountPass.NewPassword != null)
                {
                    await _unitOfWork.Account.ChangePassword(account, accountPass.NewPassword);
                    await _unitOfWork.SaveAsync();
                    return Ok("Change password successfully");
                }
                return BadRequest("You must fill all field");
            }
            return BadRequest("Account is not exists");
        }


        [HttpPut("Change-info")]
        [Authorize(Roles = StaticUserRole.USER + "," + StaticUserRole.ADMIN)]
        public async Task<ActionResult<ChangeInfoAccount>> ChangeInfo (string id, ChangeInfoAccount infoAccount)
        {
            var account = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Id == id);
            if(account != null)
            {
                _mapper.Map(infoAccount, account);
                _unitOfWork.Account.Update(account);
                var result = await _unitOfWork.SaveAsync();
                if(result > 0)
                {
                    return Ok("Update info successfully");
                }
                return BadRequest("Something wrong when updating");
            }
            return BadRequest("Account is not exists");
        }

        [HttpDelete("delete-account")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            var account = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Id == id);
            if(account != null)
            {
                _unitOfWork.Account.Delete(account);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Ok("Delete info successfully");
                }
                return BadRequest("Something wrong when delete");
            }
            return BadRequest("Can't find account to delete");
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            var account = _unitOfWork.Account.GetAccountFromAccessToken(refreshTokenDto.Token);
            var validTokenResult = await _unitOfWork.Account.ValidateRefreshToken(account, refreshTokenDto.RefreshToken);
            if (account != null && validTokenResult)
            {
                RefreshTokenDTO refreshTokenDTO = await _unitOfWork.Account.GenerateAccessToken(account);
                return Ok(new
                {
                    token = refreshTokenDTO.Token,
                    refreshToken = refreshTokenDTO.RefreshToken,
                    expiration = refreshTokenDTO.Expiration
                });
            }
            return Unauthorized();
        }
    }
}
