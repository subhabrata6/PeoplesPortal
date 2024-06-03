using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.OTTPlatform
{
    public class OTTUsersModel : PageModel
    {
        public List<string> OTTPlatforms { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Plans> Plans { get; set; } = new List<Plans>();

        [BindProperty]
        public string SelectedPlatform { get; set; }
        [BindProperty]
        public string SelectedPlan { get; set; }
        [BindProperty]
        public DateTime PurchaseDate { get; set; }
        [BindProperty]
        public string NewOTTPlatformName { get; set; }

        public void OnGet()
        {
            // Fetch OTT platforms and subscriptions from the database or any other source
            // For demonstration, I'm initializing them with sample data
            OTTPlatforms = new List<string> { "Netflix", "Amazon Prime", "Disney+", "Hulu", "Apple TV+" };
            // Subscriptions = FetchSubscriptionsFromDatabase();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(NewOTTPlatformName))
            {
                OTTPlatforms.Add(NewOTTPlatformName.Trim());
            }

            // Perform any other necessary operations, such as saving the subscription details to a database

            return RedirectToPage();
        }
    }
}
