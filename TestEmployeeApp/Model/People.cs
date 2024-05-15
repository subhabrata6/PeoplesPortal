namespace TestEmployeeApp.Model
{
    public class People
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string? description { get; set; }
        public string? state { get; set; }
        public int stateId { get; set; }
        public string? country { get; set; }
        public int countryId { get; set; }
        public string? createdOn { get; set; }
    }
}
