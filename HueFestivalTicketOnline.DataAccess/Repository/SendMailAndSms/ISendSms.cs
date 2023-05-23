using Twilio.Rest.Api.V2010.Account;

namespace HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms
{
    public interface ISendSms
    {
        MessageResource SendOtpSms(string phoneNumber, string OTP);
    }
}
