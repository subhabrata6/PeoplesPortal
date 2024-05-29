﻿using Microsoft.AspNetCore.Mvc;
using TestEmployeeWebApi.DBAccess;
using TestEmployeeWebApi.Model;

namespace TestEmployeeWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        private readonly IConfiguration _configuration;

        public PeopleController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet]
        [Route("EditPeople/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult EditPeople(int id)
        {
            ResponseMessage editResponse = new PeopleDbAccess(_configuration).GetPeopleById(id);
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
        [Route("DeletePeople/{id}")]
        public IActionResult DeletePeople(int id)
        {
            ResponseMessage editResponse = new PeopleDbAccess(_configuration).DeletePeopleById(id);
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
        [Route("GetStateList/{id}")]
        public IActionResult GetStateList(int id)
        {
            ResponseMessage editResponse = new PeopleDbAccess(_configuration).GetStateList(id);
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
