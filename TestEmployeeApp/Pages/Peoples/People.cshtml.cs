using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public int SelectedState { get; set; }

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
                NewPeople.countryId = SelectedCountry;
                NewPeople.stateId = SelectedState;

                if (!ValidEntry(NewPeople))
                {
                    // Add a model error or use the TempData to convey messages to the user if needed
                    ModelState.AddModelError(string.Empty, "The entry is not valid.");
                    OnGet();
                    return Page();
                }

                ResponseMessage response = new PeopleDbAccess(_configuration).SavePeople(NewPeople);

                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    return RedirectToPage();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data.");
                    return Page(); // Return the page to show the error
                }
            }
            else
            {
                return Forbid();
            }
        }

        private bool ValidEntry(People newPeople)
        {
            if(string.IsNullOrEmpty(newPeople.name))
            {
                TempData["AlertMessage"] = "Please enter person name!";
                return false;
            }
            if (newPeople.age <= 0)
            {
                TempData["AlertMessage"] = "Please enter valid age of person!";
                return false;
            }
            if (newPeople.countryId <= 0)
            {
                TempData["AlertMessage"] = "Please select country of person!";
                return false;
            }
            if (newPeople.stateId <= 0)
            {
                TempData["AlertMessage"] = "Please select state of person!";
                return false;
            }

            return true;
            
        }

        public void GetStateByCountry()
        {
            ResponseMessage stateResponse = new PeopleDbAccess(_configuration).GetStateList(SelectedCountry);
            States = (List<State>)stateResponse.Response;
        }

        public void deleteEntry(int id)
        {

        }

    }
}
