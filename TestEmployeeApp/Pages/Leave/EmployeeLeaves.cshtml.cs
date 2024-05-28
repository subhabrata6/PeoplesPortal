using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.Leave
{
    public class EmployeeLeavesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<LeaveModule> LeaveList { get; set; }
        public EmployeeLeavesModel(IConfiguration configuration)
        {
            LeaveList = new List<LeaveModule>();
            _configuration = configuration;
        }
        public void OnGet()
        {
            string currentuser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ResponseMessage result = new LeaveDbAccess(_configuration).GetLeaveList(currentuser);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                LeaveList = (List<LeaveModule>)result.Response;
            }
        }
    }
}
