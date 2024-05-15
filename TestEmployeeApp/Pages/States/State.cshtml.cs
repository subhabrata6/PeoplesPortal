using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.States
{
    public class StateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Country> CountryList { get; set; } = new List<Country>();
        public List<State> StateList { get; set; } = new List<State>();
        [BindProperty]
        public int SelectedCountry { get; set; }
        [BindProperty]
        public State NewState { get; set; }

        public StateModel(IConfiguration config)
        {
            _configuration = config;
            NewState = new State();
        }
        public void OnGet()
        {
            ResponseMessage response = new CountryDbAccess(_configuration).GetCountryList();
            CountryList = (List<Country>)response.Response;
            ResponseMessage stateResponse = new StateDbAccess(_configuration).GetStateList();
            StateList = (List<State>)stateResponse.Response;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                ResponseMessage response = new StateDbAccess(_configuration).SaveState(NewState);

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
