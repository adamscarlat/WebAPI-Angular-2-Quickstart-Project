using System;
using HeroAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{
     /// <summary>
     /// All authentication related action that require an API endpoint
     /// </summary>
    public class AuthController : Controller
    {
        private readonly IAuthData _authData;
        private JWTAuthTokenServices _authServices;

        public AuthController(IAuthData authData, JWTAuthTokenServices authServices)
        {
            _authData = authData;
            _authServices = authServices;
        }

        /// <summary>
        /// Logout user. Invalidate token by adding it to a blacklist. Next time 
        /// a user tries to use that token he will be rejected.
        /// </summary>
        [Route("api/auth/logout")]
        public void Logout()
        {
            var token = _authServices.ExtractJWTTokenFromHttpRequest(HttpContext.Request);

            if (string.IsNullOrEmpty(token))
                return;
            
            _authData.AddToken(token, false);
        }
    }
}