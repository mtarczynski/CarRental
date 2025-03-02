namespace CarRental.Server.Dto
{
    public class CreateReservationDto
    {
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
