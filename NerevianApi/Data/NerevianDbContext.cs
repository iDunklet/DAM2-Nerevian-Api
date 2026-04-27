using Microsoft.EntityFrameworkCore;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Offer;
using NerevianApi.Models.Business.Request;
using NerevianApi.Models.Operation;
using NerevianApi.Models.Utils;
using NerevianApi.Models.Logistics;
using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Documents;

namespace NerevianApi.Data
{
    public class NerevianDbContext : DbContext
    {
        public NerevianDbContext(DbContextOptions<NerevianDbContext> options) : base(options) { }

        // Mantenemos el nombre pero lo mapearemos abajo
        public DbSet<Notification> NotificationRequests { get; set; }
        public DbSet<ValidationType> ValidationTypes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<DNI> DNIs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RegistrationRequest> RegistrationRequests { get; set; }

        public DbSet<Operation> Operation { get; set; }
        public DbSet<OperationStatus> OperationStatuses { get; set; }

        public DbSet<CargoType> CargoTypes { get; set; }
        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FlowType> FlowTypes { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<TransportType> TransportTypes { get; set; }

        public DbSet<Incoterm> Incoterm { get; set; }
        public DbSet<IncotermType> IncotermType { get; set; }
        public DbSet<TrackingStep> TrackingStep { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<StatusOffer> StatusOffers { get; set; }

        public DbSet<Request> Requests { get; set; }
        public DbSet<StatusRequest> StatusRequests { get; set; }

        //Presupuesto
        public DbSet<Presupuesto> Presupuestos { get; set; }

        // CONFIGURACIÓN DE MAPEO DE TABLAS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esto soluciona tu error: Mapea la clase Notification a la tabla "notification"
            modelBuilder.Entity<Notification>().ToTable("notification");
        }
    }
}