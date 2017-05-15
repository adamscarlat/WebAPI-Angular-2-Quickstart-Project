using System;
using System.Threading.Tasks;
using HeroAPI.Services;
using Microsoft.AspNetCore.Mvc;

//TODO: return redirect from all POST requests
//TODO: change logout to POST
//TODO: add registration

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
        public async Task Logout()
        {
            var token = _authServices.ExtractJWTTokenFromHttpRequest(HttpContext.Request);

            if (string.IsNullOrEmpty(token))
                return;
            
            await _authData.AddToken(token, false);
        }
    }
}