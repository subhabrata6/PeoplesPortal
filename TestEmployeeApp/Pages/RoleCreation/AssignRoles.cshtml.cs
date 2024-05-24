using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.RoleCreation
{
    public class AssignRolesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<User> Users { get; set; }
        public List<UserRoleModel> UserRoles { get; set; }
        public List<AssignRole> AssignedRoles { get; set; }

        [BindProperty]
        public AssignRole Assign { get; set; }


        public AssignRolesModel(IConfiguration configuration)
        {
            Users = new List<User>();
            UserRoles = new List<UserRoleModel>();
            _configuration = configuration;
            Assign = new AssignRole();
        }

        public void OnGet()
        {
            ResponseMessage userResponse = new AccountDbAccess(_configuration).GetUserList();
            ResponseMessage rolesResponse = new AccountDbAccess(_configuration).GetRoleList();
            ResponseMessage listResponse = new AccountDbAccess(_configuration).GetUserRoleList();
            Users = (List<User>)userResponse.Response;
            UserRoles = (List<UserRoleModel>)rolesResponse.Response;
            AssignedRoles = (List<AssignRole>)listResponse.Response;
        }

        public IActionResult OnPost()
        {
            ResponseMessage response = new AccountDbAccess(_configuration).AssignRoleToUser(Assign);
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                return RedirectToPage();
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
