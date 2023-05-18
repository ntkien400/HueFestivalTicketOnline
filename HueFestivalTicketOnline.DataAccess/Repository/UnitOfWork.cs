using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            TypeProgram = new TypeProgramRepository(_dbContext);
            TypeOffline = new TypeOfflineRepository(_dbContext);
            Image = new ImageRepository(_dbContext);
            FesProgram = new FesProgramRepository(_dbContext);
            Location = new LocationRepository(_dbContext);
            MenuLocation = new MenuLocationRepository(_dbContext);
            SubMenuLocation = new SubMenuLocationRepository(_dbContext);
            DetailFesLocation = new DetailFesLocationRepository(_dbContext);
            FesTypeTicket = new FesTypeTicketRepository(_dbContext);
            InvoiceTicket = new InvoiceTicketRepository(_dbContext);
            Ticket = new TicketRepository(_dbContext);
            User = new UserRepository(_dbContext);
            News = new NewsRepository(_dbContext);
            HistoryCheck = new HistoryCheckRepository(_dbContext);
        }

        public ITypeProgramRepository TypeProgram { get; private set; }
        public ITypeOfflineRepository TypeOffline { get; private set; }
        public IImageRepository Image { get; private set; }
        public IFesProgramRepository FesProgram { get; set; }
        public ILocationRepository Location { get; set; }
        public IMenuLocationRepository MenuLocation { get; set; }
        public ISubMenuLocationRepository SubMenuLocation { get; set; }
        public IDetailFesLocationRepository DetailFesLocation { get; set; }
        public IFesTypeTicketRepository FesTypeTicket { get; set; }
        public IInvoiceTicketRepository InvoiceTicket { get; set; }
        public ITicketRepository Ticket { get; set; }
        public IUserRepository User { get; set; }
        public INewsRepository News { get; set; }
        public IHistoryCheckRepository HistoryCheck { get; set; }
        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
