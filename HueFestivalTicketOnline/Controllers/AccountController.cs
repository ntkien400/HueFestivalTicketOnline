using AutoMapper;
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
            var listAcc = _unitOfWork.Account.GetAllAsync();
            return Ok(listAcc);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<Account>> Get(string id)
        {
            var acc = await _unitOfWork.UserManager.FindByIdAsync(id);
            if (acc != null)
            {
                return Ok(acc);
            }
            return BadRequest("Account is not exists");
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(string emailOrPhone)
        {
            var validEmail = new EmailAddressAttribute().IsValid(emailOrPhone);
            var validPhone = new PhoneAttribute().IsValid(emailOrPhone);
            if (validEmail)
            {
                var acc = await _unitOfWork.UserManager.FindByEmailAsync(emailOrPhone);
                if (acc != null)
                {
                    acc.PasswordOTP = _sendSms.GenerateOTPCode(6);
                    acc.PasswordOTPExpried = DateTime.Now.AddMinutes(1);
                    await _unitOfWork.UserManager.UpdateAsync(acc);
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
                    return Ok("OTP code sent to your mail");
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
                await _unitOfWork.UserManager.RemovePasswordAsync(acc);
                await _unitOfWork.UserManager.AddPasswordAsync(acc, request.Password);
                return Ok("Change password successfully");
            }
            return BadRequest("Confirm password is not same with password");


        }

        [HttpPut("change-password")]
        [Authorize(Roles = StaticUserRole.USER +","+ StaticUserRole.ADMIN)]
        public async Task<ActionResult> ChangePassword(string id, [FromBody] ChangeAccountPass accountPass)
        {
            var account = await _unitOfWork.UserManager.FindByIdAsync(id);
            if(account != null)
            {
                if (accountPass.OldPassword != null && accountPass.NewPassword != null)
                {
                    var changePassResult = await _unitOfWork.UserManager.ChangePasswordAsync(account, accountPass.OldPassword, accountPass.NewPassword);
                    if(!changePassResult.Succeeded)
                    {
                        var errors = "Change password failed beacause: ";
                        foreach(var error in changePassResult.Errors)
                        {
                            errors += "#" + error.Description;
                        }
                        return BadRequest(errors);
                    }
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
            var account = await _unitOfWork.UserManager.FindByIdAsync(id);
            if(account != null)
            {
                _mapper.Map(infoAccount, account);
                await _unitOfWork.UserManager.UpdateAsync(account);
                await _unitOfWork.SaveAsync();
                return Ok("Update info successfully");
            }
            return BadRequest("Account is not exists");
        }

        [HttpDelete("delete-account")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            var account = await _unitOfWork.UserManager.FindByIdAsync(id);
            if(account != null)
            {
                await _unitOfWork.UserManager.DeleteAsync(account);
                return Ok("Delete account successfully");
            }
            return BadRequest("Can't find account to delete");
        }
    }
}
