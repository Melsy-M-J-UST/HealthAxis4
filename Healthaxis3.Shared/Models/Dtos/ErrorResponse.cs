using System;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.Shared.Models.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; }
    }
}
