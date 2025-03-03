using System.ComponentModel.DataAnnotations;

namespace CarRental.Server.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        public string PasswordHash { get; set; }

        public virtual List<Reservation> Reservations { get; set; }
    }
}
