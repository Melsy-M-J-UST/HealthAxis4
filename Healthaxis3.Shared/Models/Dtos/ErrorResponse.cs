using System;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxis3.Shared.Models.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}
