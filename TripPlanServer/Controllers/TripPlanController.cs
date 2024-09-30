using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripPlanServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
namespace TasksManagementServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class TripPlanAPIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private TripPlanDbContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public TripPlanAPIController(TripPlanDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("I am working!");
        }
    }
}
