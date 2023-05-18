using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IFesTypeTicketRepository : IGenericRepository<FesTypeTicket>
    {
        bool CheckTicketAmount(FesTypeTicket fesTypeTicket);
        bool UpdateTicketAmount(FesTypeTicket fesTypeTicket, int quantity);
    }
}
