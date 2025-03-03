using CarRental.Server.Entities;

namespace CarRental.Server.Services.Auth
{
    public interface IAuthService
    {
        Task<Customer> RegisterCustomer(string name, string email, string phone, string password);
        Task<string> Login(string email, string password);
    }
}
