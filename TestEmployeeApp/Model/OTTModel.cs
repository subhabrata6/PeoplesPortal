namespace TestEmployeeApp.Model
{
    public class OTTModel
    {
    }

    public class OTTListModel
    {
        public List<OttPlatforms> OTTPlatforms { get; set; } = new List<OttPlatforms>();
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public string NewOTTPlatformName { get; set; }
    }

    public class OttPlatforms
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public int ActiveUsers { get; set; }
        public string WebSiteUrl { get; set; }
    }

    public class Subscription
    {
        public string Platform { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsExpired => DateTime.Now > EndDate;
    }

    public class Plans
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlatformName { get; set; }
        public string PlatformId { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }

    }
}
