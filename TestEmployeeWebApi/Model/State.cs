namespace TestEmployeeWebApi.Model
{
    public class State
    {
        public int id { get; set; }
        public string state { get; set; }
        public int countryId { get; set; }
        public string? countryName { get; set; }
        public string? createdOn { get; set; }
    }
}
