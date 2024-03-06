namespace KycAPI.Models
{
    public class UpdateEntityRequest
    {
        public List<Address>? Addresses { get; set; }
        public List<Date> Dates { get; set; }
        public bool Deceased { get; set; }
        public string? Gender { get; set; }
        public List<Name> Names { get; set; }

        public UpdateEntityRequest()
        {
            Addresses = new List<Address>();
            Dates = new List<Date>();
            Names = new List<Name>();
        }
    }
}
