namespace CarRental.Server.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }

        public int VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
