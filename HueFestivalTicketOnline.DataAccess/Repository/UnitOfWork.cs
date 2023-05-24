using HueFestivalTicketOnline.DataAccess.Data;
using HueFestivalTicketOnline.DataAccess.Repository.IRepository;
using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
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
            AboutInformation = new AboutInformationRepository(_dbContext);
            Account = new AccountRepository(_dbContext);
        }

        public ITypeProgramRepository TypeProgram { get; private set; }
        public ITypeOfflineRepository TypeOffline { get; private set; }
        public IImageRepository Image { get; private set; }
        public IFesProgramRepository FesProgram { get; private set; }
        public ILocationRepository Location { get; private set; }
        public IMenuLocationRepository MenuLocation { get; private set; }
        public ISubMenuLocationRepository SubMenuLocation { get; private set; }
        public IDetailFesLocationRepository DetailFesLocation { get; private set; }
        public IFesTypeTicketRepository FesTypeTicket { get; private set; }
        public IInvoiceTicketRepository InvoiceTicket { get; private set; }
        public ITicketRepository Ticket { get; private set; }
        public IUserRepository User { get; private set; }
        public INewsRepository News { get; private set; }
        public IHistoryCheckRepository HistoryCheck { get; private set; }
        public IAboutInformationRepository AboutInformation { get; private set; }
        public IAccountRepository Account { get; private set; }
        public UserManager<Account> UserManager { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public async Task<int> SaveAsync()
        {
           var count = await _dbContext.SaveChangesAsync();
           return count;
        }
    }
}
