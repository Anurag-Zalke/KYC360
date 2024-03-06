using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Xml.Linq;

namespace KycAPI.Models
{
    public class Entity : IEntity
    {
        public string Id { get; set; }
        public List<Address>? Addresses { get; set; }
        public List<Date> Dates { get; set; }
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
        public List<Name> Names { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid().ToString();
            Addresses = new List<Address>();
            Dates = new List<Date>();
            Names = new List<Name>();
        }

    }
}
