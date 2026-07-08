using HealthAxis3.Shared.Models.Dtos.DoctorDtos;
using HealthAxis3.Shared.Models.Dtos.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis3.BlazorUI.Extensions
{
    public static class LinqExtensions
    {
        public static List<DoctorDto> OrderByDynamic(
            this List<DoctorDto> data,
            string column,
            bool asc)
        {
            return column switch
            {
                "name" => asc
                    ? data.OrderBy(x => x.DoctorName).ToList()
                    : data.OrderByDescending(x => x.DoctorName).ToList(),

                "experience" => asc
                    ? data.OrderBy(x => x.Experience).ToList()
                    : data.OrderByDescending(x => x.Experience).ToList(),

                "fees" => asc
                    ? data.OrderBy(x => x.Fees).ToList()
                    : data.OrderByDescending(x => x.Fees).ToList(),

                "specialisation" => asc
                    ? data.OrderBy(x => x.Specialisation).ToList()
                    : data.OrderByDescending(x => x.Specialisation).ToList(),

                _ => data
            };
        }
    }
}