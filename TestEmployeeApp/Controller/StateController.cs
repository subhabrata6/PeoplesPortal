using Microsoft.AspNetCore.Mvc;
using TestEmployeeApp.DBAccess;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.Controller
{
    [Route("[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public StateController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpGet]
        [Route("EditState/{id}")]
        public IActionResult EditState(int id)
        {
            ResponseMessage editResponse = new StateDbAccess(_configuration).GetStateById(id);
            if (editResponse.Status == System.Net.HttpStatusCode.OK)
            {
                return Ok(editResponse);
            }
            else
            {
                return NotFound(editResponse);
            }
        }

        [HttpGet]
        [Route("DeleteState/{id}")]
        public IActionResult DeleteState(int id)
        {
            ResponseMessage editResponse = new StateDbAccess(_configuration).DeleteStateById(id);
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
