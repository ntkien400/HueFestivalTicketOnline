using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms
{
    public class SendSms : ISendSms
    {
        public MessageResource SendOtpSms(string phoneNumber, string OTP)
        {
            var accountSid = "AC6b5de3af663129b8059f3c9a8ed001fc";
            var authToken = "[AuthToken]";
            var phone = phoneNumber.Remove(0, 1);
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("+84" + phone));
            messageOptions.From = new PhoneNumber("+12545408319");
            messageOptions.Body = "Mã OTP của bạn là: " + OTP;


            var message = MessageResource.Create(messageOptions);
            return message;
        }
    }
}
