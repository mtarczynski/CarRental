using CarRental.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Server.Seeders
{
    public static class CarRentalSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarRentalDbContext>();

                context.Database.Migrate();

                if(context.VehicleTypes.Any())
                {
                    return;
                }


                var vt1 = new VehicleType
                {
                    TypeName = "Osobowy",
                    Description = "Samochód osobowy"
                };
                var vt2 = new VehicleType
                {
                    TypeName = "Ciężarowy",
                    Description = "Samochód ciężarowy"
                };
                var vt3 = new VehicleType
                {
                    TypeName = "Motocykl",
                    Description = "Motocykl"
                };

                context.VehicleTypes.AddRange(vt1, vt2, vt3);
                context.SaveChanges();



                var c1 = new Customer
                {
                    Name = "Jan Kowalski",
                    Email = "jan.kowalski@example.com",
                    Phone = "123456789"
                };
                var c2 = new Customer
                {
                    Name = "Adam Nowak",
                    Email = "adam.nowak@example.com",
                    Phone = "123456789"
                };

                context.Customers.AddRange(c1, c2);
                context.SaveChanges();


                var v1 = new Vehicle
                {
                    Brand = "Renault",
                    Model = "Clio",
                    Year = 2024,
                    RegistrationNumber = "ABC",
                    VehicleTypeId = vt1.Id
                };                   
                var v2 = new Vehicle
                {
                    Brand = "DEF",
                    Model = "XF 480",
                    Year = 2021,
                    RegistrationNumber = "XYZ",
                    VehicleTypeId = vt2.Id
                };
                var v3 = new Vehicle
                {
                    Brand = "Yamaha",
                    Model = "R1",
                    Year = 2022,
                    RegistrationNumber = "ASD",
                    VehicleTypeId = vt3.Id
                };

                context.Vehicles.AddRange(v1, v2, v3);
                context.SaveChanges();



                var r1 = new Reservation
                {
                    VehicleId = v1.Id,
                    CustomerId = c1.Id,
                    StartTime = new DateTime(2025, 03, 02, 8, 0, 0),
                    EndTime = new DateTime(2025, 03, 16, 8, 0, 0),
                    Status = "Active",
                    CreatedAt = DateTime.Now
                };

                context.Reservations.Add(r1);
                context.SaveChanges();
            }
        }
    }
}
