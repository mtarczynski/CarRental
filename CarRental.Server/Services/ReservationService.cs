using CarRental.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Server.Services
{
    public class ReservationService : IReservationService
    {
        private readonly CarRentalDbContext _context;

        public ReservationService(CarRentalDbContext context)
        {
            _context = context;
        }
        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            if(!await IsVehicleAvailableAsync(reservation.VehicleId, reservation.StartTime, reservation.EndTime))
            {
                throw new Exception("Samochód jest zarezerwowany w tym przedziale czasowym.");
            }

            reservation.CreatedAt = DateTime.Now;
            reservation.Status = "Active";
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation;

        }

        public async Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId)
        {
            return await _context.Reservations
                .Include(r => r.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime starTime, DateTime endTime)
        {
            bool isBooked = await _context.Reservations.AnyAsync(r =>
            r.VehicleId == vehicleId &&
            r.EndTime > starTime &&
            r.StartTime < endTime &&
            r.Status == "Active"
            );

            return !isBooked;
        }

        public async Task<Reservation> ReturnVehicleAsync(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if(reservation == null)
            {
                throw new Exception($"Rezerwacja o ID {reservationId} nie istnieje");
            }
            if(reservation.Status != "Active")
            {
                throw new Exception($"Rezerwacja o ID {reservationId} nie jest aktywna");
            }

            reservation.Status = "Returned";
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }
    }
}
