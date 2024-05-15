using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Peoples
{
    public class PeopleModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<People> People { get; set; } = new List<People>();
        [BindProperty]
        public People NewPeople { get; set; }
        [BindProperty]
        public List<Country> Countries { get; set; }
        [BindProperty]
        public int SelectedCountry { get; set; }

        [BindProperty]
        public List<State> States { get; set; }

        public PeopleModel(IConfiguration config)
        {
            _configuration = config;
            NewPeople = new People();
            States = new List<State>();
            Countries = new List<Country>();
        }
        public void OnGet()
        {
            ResponseMessage peopleResponse = new PeopleDbAccess(_configuration).GetPeopleList();
            ResponseMessage countryResponse = new PeopleDbAccess(_configuration).GetCountryList();
            People = (List<People>)peopleResponse.Response;
            Countries = (List<Country>)countryResponse.Response;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                ResponseMessage response = new PeopleDbAccess(_configuration).SavePeople(NewPeople);

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
                return Forbid();
            }
        }

        public void GetStateByCountry()
        {
            ResponseMessage stateResponse = new PeopleDbAccess(_configuration).GetStateList(SelectedCountry);
            States = (List<State>)stateResponse.Response;
        }

        public void editEntry(int id)
        {

        }
        public void deleteEntry(int id)
        {

        }

    }
}
