using AutoMapper;
using CarRental.Server.Dto;
using CarRental.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Server.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehiclesController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableVehicles([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var vehicles = await _vehicleService.GetAvailableVehiclesAsync(start, end);
                var result = _mapper.Map<List<VehicleDto>>(vehicles);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
