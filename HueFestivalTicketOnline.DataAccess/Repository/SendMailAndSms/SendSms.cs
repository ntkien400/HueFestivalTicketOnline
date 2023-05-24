using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            var accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            var authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            var phone = phoneNumber.Remove(0, 1);
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
              new PhoneNumber("+84" + phone));
            messageOptions.From = new PhoneNumber("+12545408319");
            messageOptions.Body = "Mã OTP của bạn là: " + OTP;


            var message = MessageResource.Create(messageOptions);
            return message;
        }

        public string GenerateOTPCode(int length)
        {
            const string valid = "1234567890";
            StringBuilder sb = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    sb.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return sb.ToString();
        }
    }
}
