using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Leave
{
    public class ApplyLeaveModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public LeaveModule NewLeave { get; set; }

        public ApplyLeaveModel(IConfiguration configuration)
        {
            NewLeave = new LeaveModule();
            _configuration = configuration;

        }


        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            string currentuser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            NewLeave.CreatedBy = currentuser;

            ResponseMessage result = new LeaveDbAccess(_configuration).SaveLeave(NewLeave);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage("/Leave/EmployeeLeaves");
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
