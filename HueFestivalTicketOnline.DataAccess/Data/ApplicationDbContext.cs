using HueFestivalTicketOnline.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueFestivalTicketOnline.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<FesProgram> FesPrograms { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<SubMenuLocation> SubMenuLocations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<HistoryCheck> HistoryChecks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FesTypeTicket> FesTypeTickets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TypeOffline> TypeOfflines { get; set; }
        public DbSet<TypeProgram> TypePrograms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<AboutInformation> AboutInformations { get; set; }
        public DbSet<MenuLocation> MenuLocations { get; set; }
        public DbSet<DetailFesLocation> DetailFesLocations { get; set; }
        public DbSet<InvoiceTicket> InvoiceTickets { get; set; }
    }
}
