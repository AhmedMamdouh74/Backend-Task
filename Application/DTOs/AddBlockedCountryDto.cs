using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public record AddBlockedCountryDto
    {
        [Required]
        [StringLength(2, MinimumLength = 2)]
        public string CountryCode { get; set; }
    }
}
