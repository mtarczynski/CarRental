using CarRental.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRental.Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly CarRentalDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(CarRentalDbContext context, IConfiguration confiugration)
        {
            _context = context;
            _configuration = confiugration;
        }

        public async Task<string> Login(string email, string password)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
            {
                throw new Exception("Invalid email or password.");
            }
            var passwordHash = HashPassword(password);
            if (customer.PasswordHash != passwordHash)
            {
                throw new Exception("Invalid email or password.");
            }
            return GenerateJwtToken(customer);
        }

        private string GenerateJwtToken(Customer customer)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim("Phone", customer.Phone ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async Task<Customer> RegisterCustomer(string name, string email, string phone, string password)
        {
            var existing = await _context.Customers.AnyAsync(c => c.Email == email);
            if (existing)
            {
                throw new Exception("Istnieje konto z takim adresem Email");
            }
            var passwordHash = HashPassword(password);

            var customer = new Customer
            {
                Name = name,
                Email = email,
                Phone = phone,
                PasswordHash = passwordHash
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
