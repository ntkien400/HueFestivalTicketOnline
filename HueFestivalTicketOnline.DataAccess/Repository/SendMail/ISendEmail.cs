using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HueFestivalTicketOnline.DataAccess.Repository.SendMail
{
    public interface ISendEmail
    {
        Task SendEmailAsync(string email, List<string> listInfo);
    }
}
