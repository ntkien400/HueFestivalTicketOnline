using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using QRCoder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Drawing;
using System.Net.Sockets;
using System.Text;

namespace HueFestivalTicketOnline.DataAccess.Repository.SendMailAndSms
{
    public class SendEmail : ISendEmail
    {
        private const string pathFolder = "C:/Users/ntkie/OneDrive/Pictures/QRCode/";
        public Task SendEmailAsync(string email, List<string> listInfo)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ntkien400@gmail.com", "FestivalHue");
            var subject = "Thanh toán vé thành công";
            var to = new EmailAddress(email, "User");
            var plainTextContent = "Success";
            var htmlContent = "Đây là vé bạn đã đặt mua";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //Attach qr ticket image to email
            var count = listInfo.Count;
            while (count-- > 0)
            {
                var str = listInfo[count].Split("|");
                var ticketName = str[0] + ".png";
                var fileName = GenerateQRCodeAndGetImageName(listInfo[count]);
                var bytes = File.ReadAllBytes(pathFolder + fileName);
                var file = Convert.ToBase64String(bytes);
                msg.AddAttachment(ticketName, file, "image/png", "inline", "qrImage");
            }
            return client.SendEmailAsync(msg);
        }

        public Task SendForgotPasswordEmailAsync(string email, string otpCode)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ntkien400@gmail.com", "FestivalHue");
            var subject = "Mã khôi phục mật khẩu";
            var to = new EmailAddress(email, "User");
            var plainTextContent = "Success";
            var htmlContent = "Mã chỉ có hiệu lực trong 10p, mã khôi phục của bạn là: " + otpCode;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //Attach qr ticket image to email
            
            return client.SendEmailAsync(msg);
        }
        private string GenerateQRCodeAndGetImageName(string ticketInfo)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(ticketInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(5);
            var fileName = Guid.NewGuid().ToString() + ".png";
            qrImage.Save(pathFolder + fileName, System.Drawing.Imaging.ImageFormat.Png);
            return fileName;
        }


    }
}
