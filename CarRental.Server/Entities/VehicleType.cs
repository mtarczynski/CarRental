namespace CarRental.Server.Entities
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }

        public virtual List<Vehicle> Vehicles { get; set; }
    }
}
