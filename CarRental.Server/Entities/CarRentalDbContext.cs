using Microsoft.EntityFrameworkCore;

namespace CarRental.Server.Entities
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }    
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasMany(v => v.Reservations)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(v => v.RegistrationNumber)
                .IsRequired();
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasMany(vt => vt.Vehicles)
                .WithOne(v => v.VehicleType)
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(vt => vt.TypeName)
                .IsRequired()
                .HasMaxLength(50);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasMany(c => c.Reservations)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

                entity.Property(c => c.Email)
                .IsRequired();
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(r => r.StartTime)
                .IsRequired();

                entity.Property(r => r.EndTime)
                .IsRequired();

                entity.Property(r => r.Status)
                .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
