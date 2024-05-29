using Microsoft.AspNetCore.Mvc;
using TestEmployeeWebApi.DBAccess;
using TestEmployeeWebApi.Model;

namespace TestEmployeeWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private readonly IConfiguration _configuration;
        public CountryController(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        [HttpGet]
        [Route("EditCountry/{id}")]
        public IActionResult EditCountry(int id)
        {
            ResponseMessage editResponse = new CountryDbAccess(_configuration).GetCountryById(id);
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
        [Route("DeleteCountry/{id}")]
        public IActionResult DeleteCountry(int id)
        {
            ResponseMessage editResponse = new CountryDbAccess(_configuration).DeleteCountryById(id);
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
