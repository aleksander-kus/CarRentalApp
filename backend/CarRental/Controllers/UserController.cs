using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports;
using CarRental.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<UserDetails> GetCurrentUser()
        {
            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                var all = identity.Claims.ToList();
                var userId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

                if (userId == null)
                {
                    throw new ArgumentException("userId not present");
                }

                return await _getUserDetailsUseCase.GetUserDetails(userId);
            }

            return null;
        }
    }
}