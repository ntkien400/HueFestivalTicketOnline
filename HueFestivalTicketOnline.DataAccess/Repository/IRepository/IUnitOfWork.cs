using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ITypeProgramRepository TypeProgram { get; }
        ITypeOfflineRepository TypeOffline { get; }
        IImageRepository Image { get; }
        IFesProgramRepository FesProgram { get; }
        ILocationRepository Location { get; }
        IMenuLocationRepository MenuLocation { get; }
        ISubMenuLocationRepository SubMenuLocation { get; }
        IDetailFesLocationRepository DetailFesLocation { get; }
        IFesTypeTicketRepository FesTypeTicket { get; }
        IInvoiceTicketRepository InvoiceTicket { get; }
        ITicketRepository Ticket { get; }
        IUserRepository User { get; }
        INewsRepository News { get; }
        IHistoryCheckRepository HistoryCheck {get;}
        Task DisposeAsync();
        Task SaveAsync();
    }
}
