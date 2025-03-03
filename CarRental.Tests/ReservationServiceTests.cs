using CarRental.Server.Entities;
using CarRental.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Tests
{
    public class ReservationServiceTests
    {
        [Fact]
        public async Task ShouldThrowException_WhenDoubleBooking()
        {
            var options = new DbContextOptionsBuilder<CarRentalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new CarRentalDbContext(options))
            {
                var vehicle = new Vehicle
                {
                    Brand = "Toyota",
                    Model = "Corolla",
                    Year = 2022,
                    RegistrationNumber = "ABC123"
                };
                context.Vehicles.Add(vehicle);
                await context.SaveChangesAsync();

                var existingReservation = new Reservation
                {
                    VehicleId = vehicle.Id,
                    CustomerId = 1,
                    StartTime = new DateTime(2025, 03, 01, 10, 0, 0),
                    EndTime = new DateTime(2025, 03, 01, 12, 0, 0),
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };
                context.Reservations.Add(existingReservation);
                await context.SaveChangesAsync();

                var reservationService = new ReservationService(context);

                var overlappingReservation = new Reservation
                {
                    VehicleId = vehicle.Id,
                    CustomerId = 2,
                    StartTime = new DateTime(2025, 03, 01, 11, 0, 0),
                    EndTime = new DateTime(2025, 03, 01, 13, 0, 0),
                    Status = "Active"
                };

                await Assert.ThrowsAsync<Exception>(async () =>
                {
                    await reservationService.CreateReservationAsync(overlappingReservation);
                });
            }
        }
    }
}
