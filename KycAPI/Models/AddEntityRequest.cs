namespace KycAPI.Models
{
    public class AddEntityRequest
    {
        public List<Address>? Addresses { get; set; }
        public List<Date> Dates { get; set; }
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
        public List<Name> Names { get; set; }

        public AddEntityRequest()
        {
            Addresses = new List<Address>();
            Dates = new List<Date>();
            Names = new List<Name>();
        }
    }
}
