namespace CarRental.Server.Dto
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public int VehicleId { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
