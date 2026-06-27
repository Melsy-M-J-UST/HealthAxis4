using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis3.Shared.Models.Dtos.AppointmentDtos
{
        public class AppointmentReportDto
        {
            public DateTime Date { get; set; }
            public int CompletedCount { get; set; }
            public int CancelledCount { get; set; }
            public int ConfirmedCount { get; set; }
        }
}
