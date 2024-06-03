using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.OTTPlatform
{
    public class OTTListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<OttPlatforms> OTTPlatforms { get; set; }
        [BindProperty]
        public string SelectedPlatform { get; set; }
        [BindProperty]
        public OttPlatforms NewOTTPlatform { get; set; }

        public OTTListModel(IConfiguration configuration)
        {
            _configuration = configuration;
            OTTPlatforms = new List<OttPlatforms>();
            NewOTTPlatform = new OttPlatforms();
        }

        public void OnGet()
        {
            ResponseMessage response = new OTTDBAccess(_configuration).GetOttList();
            OTTPlatforms = (List<OttPlatforms>)response.Response;
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(NewOTTPlatform.PlatformName))
            {
                ResponseMessage response = new OTTDBAccess(_configuration).SaveOTT(NewOTTPlatform);
            }

            return RedirectToPage();
        }
    }
}
