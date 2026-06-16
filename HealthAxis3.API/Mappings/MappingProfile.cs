using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos;

namespace HealthAxis3.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();
            CreateMap<Doctor, DoctorDto>().ReverseMap();
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<HealthRecord, HealthRecordDto>().ReverseMap();
        }
    }
}
