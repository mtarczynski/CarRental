using AutoMapper;
using CarRental.Server.Dto;
using CarRental.Server.Entities;
using CarRental.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Server.Controllers
{
    [ApiController]
    [Route("api/reservation")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            try
            {
                var reservationEntity = _mapper.Map<Reservation>(dto);

                var createdReservation = await _reservationService.CreateReservationAsync(reservationEntity);

                var resultDto = _mapper.Map<ReservationDto>(createdReservation);
                return Ok(resultDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/{id}/return")]
        public async Task<IActionResult>ReturnVehicle(int id)
        {
            try
            {
                var updatedReservation = await _reservationService.ReturnVehicleAsync(id);
                var resultDto = _mapper.Map<ReservationDto>(updatedReservation);
                return Ok(resultDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
