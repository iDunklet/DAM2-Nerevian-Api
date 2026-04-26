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

        // --- DbSets ---
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

        // --- Configuración de Mapeo (OnModelCreating) ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzamos a Entity Framework a buscar nombres en singular o específicos
            // para que coincidan con tu base de datos SQL Server

            modelBuilder.Entity<Notification>().ToTable("notification");
            modelBuilder.Entity<ContainerType>().ToTable("ContainerType"); // Corrige el Error 500
            modelBuilder.Entity<CargoType>().ToTable("CargoType");
            modelBuilder.Entity<Port>().ToTable("Port");
            modelBuilder.Entity<OperationStatus>().ToTable("OperationStatus");
            modelBuilder.Entity<Operation>().ToTable("Operation");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Request>().ToTable("Request");
            modelBuilder.Entity<Offer>().ToTable("Offer");
            modelBuilder.Entity<Incoterm>().ToTable("Incoterm");
            modelBuilder.Entity<Carrier>().ToTable("Carrier");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<FlowType>().ToTable("FlowType");
            modelBuilder.Entity<TransportType>().ToTable("TransportType");
            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<DocumentType>().ToTable("DocumentType");

            // Si alguna tabla tiene un nombre distinto (ej. todo minúsculas),
            // cámbialo en el segundo parámetro del .ToTable("")
        }
    }
}