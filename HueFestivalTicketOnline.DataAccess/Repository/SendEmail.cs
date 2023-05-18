using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using QRCoder;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Drawing;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class SendEmail : ISendEmail
    {
        public Task SendEmailAsync(string email, Ticket ticket)
        {
            //GenerateQRCode(ticket.TicketInfo);
            var filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            StreamReader sr = new StreamReader(filePath +"/SendGridKey.txt");
            var sendGridKey = sr.ReadLine(); 
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("ntkien402@gmail.com", "FestivalHue");
            var subject = "Thanh toán vé thành công";
            var to = new EmailAddress(email, "User");
            var plainTextContent = "Đây là vé của bạn:" ;
            var htmlContent =  $"<img src='data:image/png;base64' />";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg);
        }
        private void GenerateQRCode(string ticketInfo)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(ticketInfo, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrImage = qrCode.GetGraphic(5);
            var fileName = Guid.NewGuid().ToString();
            qrImage.Save("C:/Users/ntkie/OneDrive/Pictures/QRCode/" + fileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }


    }
}
