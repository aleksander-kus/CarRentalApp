using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CarRental
{
    public static class TokenClaimsUtil
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity is ClaimsIdentity identity)
            {
                var all = identity.Claims.ToList();
                var userId = identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

                if (userId == null)
                {
                    throw new ArgumentException("userId not present");
                }

                return userId;
            }

            return null;
        }
    }
}