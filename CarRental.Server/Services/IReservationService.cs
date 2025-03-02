using CarRental.Server.Entities;

namespace CarRental.Server.Services
{
    public interface IReservationService
    {
        Task<bool> IsVehicleAvailableAsync(int vehicleId, DateTime starTime, DateTime endTime);

        Task<Reservation> CreateReservationAsync(Reservation reservation);

        Task<Reservation> ReturnVehicleAsync(int reservationId);
    }
}
