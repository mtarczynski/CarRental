
using CarRental.Server.Entities;
using CarRental.Server.Mapping;
using CarRental.Server.Seeders;
using CarRental.Server.Services;
using CarRental.Server.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rotativa.AspNetCore;
using System.Text;

namespace CarRental.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings.GetValue<string>("SecretKey");
            string issuer = jwtSettings.GetValue<string>("Issuer");
            string audience = jwtSettings.GetValue<string>("Audience");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowLocalhost54490",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:54490")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });
            builder.Services.AddDbContext<CarRentalDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CarRentalDbConnection")));
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseCors("AllowLocalhost54490");
            RotativaConfiguration.Setup(app.Environment.WebRootPath, "Rotativa");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    CarRentalSeeder.Initialize(services);
                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Wyst¹pi³ b³¹d podczas seedowania");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
