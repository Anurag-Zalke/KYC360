using System.ComponentModel.DataAnnotations;

namespace KycAPI.Models
{
    public class Name
    {
        [Key]
        public int NameId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? Surname { get; set; }
    }
}
