using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TestEmployeeWebApi.DBAccess;
using TestEmployeeWebApi.Model;

namespace TestEmployeeWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTTController : Controller
    {
        private readonly IConfiguration _configuration;

        public OTTController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("SaveSubscriptionPlan")]
        public IActionResult SaveSubscriptionPlan(Plans plan)
        {
            ResponseMessage editResponse = new OTTDbAccess(_configuration).SaveSubscriptionPlan(plan);
            if (editResponse.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(editResponse);
            }
            else
            {
                return NotFound(editResponse);
            }
        }
    }
}
