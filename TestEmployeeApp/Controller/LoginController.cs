using Microsoft.AspNetCore.Mvc;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Controller
{
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost]
        [Route("AuthenticateUser")]
        public IActionResult AuthenticateUser([FromBody]AccountModel account)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).AuthenticateUser(account);
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
        [Route("RegisterUser")]
        public IActionResult RegisterUser([FromBody] AccountModel account)
        {
            ResponseMessage result = new AccountDbAccess(_configuration).RegisterUser(account);
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
