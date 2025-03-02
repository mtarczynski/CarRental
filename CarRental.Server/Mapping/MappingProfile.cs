using AutoMapper;
using CarRental.Server.Dto;
using CarRental.Server.Entities;

namespace CarRental.Server.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateReservationDto, Reservation>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.VehicleRegistrationNumber, opt => opt.MapFrom(src => src.Vehicle.RegistrationNumber));

            CreateMap<Vehicle, VehicleDto>()
                .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.TypeName));
        }
    }
}
