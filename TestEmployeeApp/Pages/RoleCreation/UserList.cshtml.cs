using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Pages.RoleCreation
{
    public class UserListModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<UserView> Users { get; set; }

        public UserListModel(IConfiguration configuration)
        {
            Users = new List<UserView>();
            _configuration = configuration;
        }
        public void OnGet()
        {
            ResponseMessage userResponse = new AccountDbAccess(_configuration).GetUsersViewList();
            Users = (List<UserView>) userResponse.Response;
        }
    }
}
