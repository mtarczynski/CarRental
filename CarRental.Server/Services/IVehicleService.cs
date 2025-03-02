using CarRental.Server.Entities;

namespace CarRental.Server.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAvailableVehiclesAsync(DateTime start, DateTime end);
    }
}
