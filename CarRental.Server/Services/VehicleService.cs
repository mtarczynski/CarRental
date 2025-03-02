using CarRental.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Server.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly CarRentalDbContext _conext;

        public VehicleService(CarRentalDbContext conext)
        {
            _conext = conext;
        }

        public async Task<List<Vehicle>> GetAvailableVehiclesAsync(DateTime start, DateTime end)
        {
            var vehicles = await _conext.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.Reservations)
                .Where(v => !v.Reservations.Any(r =>
                    r.Status == "Active" &&
                    r.EndTime > start &&
                    r.StartTime < end
                ))
                .ToListAsync();

            return vehicles;
        }
    }
}
