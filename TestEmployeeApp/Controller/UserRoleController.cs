using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Controller
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserRoleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetUserRoleList")]
        public IActionResult GetUserRoleList()
        {
            ResponseMessage result = new AccountDbAccess(_configuration).GetRoleList();
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("EditRole/{id}")]
        public IActionResult EditRole(int id)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).GetUserRoleById(id);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("DeleteUserRole/{id}")]
        public IActionResult DeleteUserRole(int id)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).DeleteUserRoleById(id);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("SaveRole")]
        public IActionResult SaveRole([FromBody] UserRoleModel role)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).SaveUserRole(role);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("EditUserRole/{id}")]
        public IActionResult EditUserRole(int id)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).GetAssignedUserRoleById(id);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("DeleteAssignedRoleToUser/{id}")]
        public IActionResult DeleteAssignedRoleToUser(int id)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).DeleteAssignedRoleToUser(id);
            if (result.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }
    }
}
