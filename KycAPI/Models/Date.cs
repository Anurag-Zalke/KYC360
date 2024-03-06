using System;
using System.ComponentModel.DataAnnotations;

namespace KycAPI.Models
{
    public class Date
    {
        [Key]
        public int DateId { get; set; }
        public string? DateType { get; set; }
        public DateTime? DateValue { get; set; }
    }
}
