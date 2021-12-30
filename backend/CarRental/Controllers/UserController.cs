using System;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/user")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGetUserDetailsUseCase _getUserDetailsUseCase;

        public UserController(IGetUserDetailsUseCase getUserDetailsUseCase)
        {
            _getUserDetailsUseCase = getUserDetailsUseCase;
        }

        [HttpGet]
        public async Task<ActionResult<UserDetails>> GetCurrentUser()
        {
            var userId = HttpContext.User.GetUserId();

            return userId != null ? Ok(await _getUserDetailsUseCase.GetUserDetailsAsync(userId)) : NoContent();
        }
    }
}