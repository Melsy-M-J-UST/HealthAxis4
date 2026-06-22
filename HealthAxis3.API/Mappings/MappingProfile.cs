using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.Shared.Models.Dtos.AppointmentDtos;
using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.Shared.Models.Dtos.HealthrecordDtos;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;

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
            CreateMap<Patient, PatientUpdateDto>().ReverseMap();
            CreateMap<Doctor, DoctorUpdateDto>().ReverseMap();
            CreateMap<Appointment, AppointmentUpdateDto>().ReverseMap();

        }
    }
}
