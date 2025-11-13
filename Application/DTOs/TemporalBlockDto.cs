using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record TemporalBlockDto
    {
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }

        [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
        public int DurationMinutes { get; set; }
    }

}
