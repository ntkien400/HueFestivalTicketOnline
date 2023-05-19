using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext= dbContext;
        }

        public int GetTicketIdByTicketCode(string ticketCode)
        {
            return _dbContext.Tickets.Where(t => t.TicketCode == ticketCode).Select(t => t.Id).SingleOrDefault();
            
        }
    }
}
