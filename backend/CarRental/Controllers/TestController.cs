using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Client")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<String> Hello()
        {
            return "Hello";
        }
    }
}