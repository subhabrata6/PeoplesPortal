using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.OTTPlatform
{
    public class SubscriptionListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<OttPlatforms> OTTPlatformNames { get; set; }
        public List<Plans> SubscriptionPlans { get; set; } = new List<Plans>();
        public SubscriptionListModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            ResponseMessage response = new OTTDBAccess(_configuration).GetOttList();
            OTTPlatformNames = (List<OttPlatforms>)response.Response;
        }
        public void OnPost()
        {

        }
    }
}
