namespace CarRental.Server.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationNumber { get; set; }

        public int VehicleId { get; set; }
        public string VehicleTypeName { get; set; }
    }
}
