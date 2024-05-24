using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            
        }
    }
}
