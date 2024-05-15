using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Countries
{
    public class CountryModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Country> Country { get; set; } = new List<Country>();
        [BindProperty]
        public Country NewCountry { get; set; }

        public CountryModel(IConfiguration config)
        {
            _configuration = config;
            NewCountry = new Country();
            Country = new List<Country>();
        }
        public void OnGet()
        {
        }
    }
}
