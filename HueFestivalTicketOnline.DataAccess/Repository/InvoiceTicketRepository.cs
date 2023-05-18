using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class InvoiceTicketRepository : GenericRepository<InvoiceTicket>, IInvoiceTicketRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceTicketRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
    }
}
