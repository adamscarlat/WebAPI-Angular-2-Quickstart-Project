using System;
using HeroAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HeroAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthData _authData;

        public AuthController(IAuthData authData)
        {
            _authData = authData;
        }

        [Route("api/auth/logout")]
        public void Logout()
        {
            //TODO: get jwt token from request and invalidate it (if it's in the DB)

            StringValues authHeader = string.Empty;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authHeader);

            var authHeaderArray = authHeader.ToString().Split(' ');
            var authToken = string.Empty;
            if (authHeaderArray.Length > 1)
                authToken = authHeaderArray[1];
            
            Console.WriteLine(authToken);
            var tokenStore = _authData.GetToken(authToken);

            if (tokenStore != null)
                tokenStore.IsValid = false;

            _authData.Commit();

            //Console.WriteLine(tokenStore.IsValid);
        }
    }
}