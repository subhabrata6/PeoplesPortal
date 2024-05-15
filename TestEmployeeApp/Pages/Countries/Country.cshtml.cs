using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Countries
{
    public class CountryModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Country> CountryList { get; set; } = new List<Country>();
        [BindProperty]
        public Country NewCountry { get; set; }

        public CountryModel(IConfiguration config)
        {
            _configuration = config;
            NewCountry = new Country();
        }
        public void OnGet()
        {
            ResponseMessage response = new CountryDbAccess(_configuration).GetCountryList();
            CountryList = (List<Country>)response.Response;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                ResponseMessage response = new CountryDbAccess(_configuration).SaveCountry(NewCountry);

                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return Forbid();
            }
        }
    }
}
