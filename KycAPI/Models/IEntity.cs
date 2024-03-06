using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace KycAPI.Models
{
    public interface IEntity
    {
        string Id { get; set; }
        List<Address>? Addresses { get; set; }
        List<Date> Dates { get; set; }
        bool Deceased { get; set; }
        string? Gender { get; set; }
        List<Name> Names { get; set; }
    }
}
