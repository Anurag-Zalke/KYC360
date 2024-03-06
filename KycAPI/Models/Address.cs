using System.ComponentModel.DataAnnotations;

namespace KycAPI.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string? AddressLine { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
