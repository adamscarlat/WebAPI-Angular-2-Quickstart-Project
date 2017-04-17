using System;
using HeroAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeroAPI.Controllers
{
    /// <summary>
    /// API for authentication related request
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
        /// Logout API. If a token is attached to request it will invalidate it
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