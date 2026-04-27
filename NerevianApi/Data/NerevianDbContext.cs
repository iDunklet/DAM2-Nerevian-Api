using Microsoft .EntityFrameworkCore;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Offer;
using NerevianApi.Models.Business.Request;
using NerevianApi.Models.Operations;
using NerevianApi.Models.Utils;
using NerevianApi.Models.Logistics;
using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Documents;

namespace NerevianApi.Data
{
    
    public class NerevianDbContext : DbContext
    {
        // Contructor
        public NerevianDbContext(DbContextOptions<NerevianDbContext> options) : base(options)
        {
        }

        // Utils 
        public DbSet<Notification> NotificationRequests { get; set; }
        public DbSet<ValidationType> ValidationTypes { get; set; }


        // User
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DNI> DNIs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }


        // Operations
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationStatus> OperationStatuses { get; set; }


        // Logistics
        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FlowType> FlowTypes { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }


        // Incoterms
        public DbSet<Incoterm> Incoterm { get; set; }
        public DbSet<IncotermType> IncotermType { get; set; }
        public DbSet<TrackingStep> TrackingStep { get; set; }


        // Documents
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }


        // Business - Offer
        public DbSet<Offer> Offers { get; set; }
        public DbSet<StatusOffer> StatusOffers { get; set; }


        // Business - Request
        public DbSet<Request> Requests { get; set; }
        public DbSet<StatusRequest> StatusRequests { get; set; }

        //Presupuesto
        public DbSet<Presupuesto> Presupuestos { get; set; }





    }
}
