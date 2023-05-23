using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms;
using HueFestivalTicketOnline.Models.DTOs;
using HueFestivalTicketOnline.Models.DTOs.Authentiction;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static QRCoder.PayloadGenerator;

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
        private readonly UserManager<Account> _userManager;

        public AccountController(IUnitOfWork unitOfWork, ISendEmail sendEmail, ISendSms sendSms, UserManager<Account> userManager)
        {
            _unitOfWork = unitOfWork;
            _sendEmail = sendEmail;
            _userManager = userManager;
            _sendSms = sendSms;
        }

        [HttpGet]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Account>> GetAllAccount()
        {
            var listAcc = _unitOfWork.Account.GetAllAsync();
            return Ok(listAcc);
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var acc = await _unitOfWork.Account.GetAsync(id);
            if(acc != null)
            {
                return Ok(acc);
            }
            return BadRequest("Account is not exists");
        }

        // POST api/<AccountController>
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(string emailOrPhone)
        {
            var validEmail = new EmailAddressAttribute().IsValid(emailOrPhone);
            var validPhone = new PhoneAttribute().IsValid(emailOrPhone);
            if(validEmail)
            {
                var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.Email == emailOrPhone);
                if (acc != null)
                {
                    acc.PasswordOTP = _unitOfWork.Account.GenerateOTPCode(8);
                    acc.PasswordOTPExpried = DateTime.Now.AddMinutes(1);
                    _unitOfWork.Account.Update(acc);
                    await _unitOfWork.SaveAsync();
                    await _sendEmail.SendForgotPasswordEmailAsync(emailOrPhone, acc.PasswordOTP);
                    return Ok("OTP code sent to your mail");
                }
            }
            if(validPhone)
            {
                var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.PhoneNumber == emailOrPhone);
                if (acc != null)
                {
                    acc.PasswordOTP = _unitOfWork.Account.GenerateOTPCode(8);
                    acc.PasswordOTPExpried = DateTime.Now.AddMinutes(1);
                    _unitOfWork.Account.Update(acc);
                    await _unitOfWork.SaveAsync();
                    _sendSms.SendOtpSms(emailOrPhone, acc.PasswordOTP);
                    return Ok("OTP code sent to your mail");
                }
            }
            return BadRequest("Email or Phone invalid");
        }
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto request)
        {
            var acc = await _unitOfWork.Account.GetFirstOrDefaultAsync(a => a.PasswordOTP == request.OTPCode);
            if (acc == null || acc.PasswordOTPExpried < DateTime.Now)
            {
                return BadRequest("Invalid OTP");
            }
            if(request.Password == request.ConfirmPassword)
            {
                await _userManager.RemovePasswordAsync(acc);
                await _userManager.AddPasswordAsync(acc, request.Password);
                return Ok("Change password successfully");
            }
            return BadRequest("Confirm password is not same with password");
            

        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
