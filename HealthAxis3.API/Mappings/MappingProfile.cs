using AutoMapper;
using HealthAxis3.API.Models;
using HealthAxis3.API.Models.Dtos.AppointmentDto;
using HealthAxis3.API.Models.Dtos.DoctorDto;
using HealthAxis3.API.Models.Dtos.HealthrecordDto;
using HealthAxis3.API.Models.Dtos.PatientDto;

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
