using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public string Home()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

    }
}
