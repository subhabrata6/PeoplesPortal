using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.RoleCreation
{
    [Authorize(Roles = "Admin")]
    public class UserRolesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public UserRoleModel Role { get; set; }

        public List<UserRoleModel> RoleList { get; set; } = new List<UserRoleModel>();
        public UserRolesModel(IConfiguration configuration)
        {
            _configuration = configuration;
            Role = new UserRoleModel();
        }
        public void OnGet()
        {
            ResponseMessage response = new AccountDbAccess(_configuration).GetRoleList();
            RoleList = (List<UserRoleModel>)response.Response;
        }
    }
}
