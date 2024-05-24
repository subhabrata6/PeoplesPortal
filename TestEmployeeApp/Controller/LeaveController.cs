using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Controller
{
    [Route("[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LeaveController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult SaveLeave(LeaveModule module)
        {
            string currentuser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            module.CreatedBy = currentuser;
            ResponseMessage result = new LeaveDbAccess(_configuration).SaveLeave(module);
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
