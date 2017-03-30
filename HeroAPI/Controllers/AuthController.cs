using System;
using HeroAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HeroAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthData _authData;
        private JWTAuthTokenServices _authServices;

        public AuthController(IAuthData authData, JWTAuthTokenServices authServices)
        {
            _authData = authData;
            _authServices = authServices;
        }

        [Route("api/auth/logout")]
        public void Logout()
        {
            Console.WriteLine("in logout controller...");

            var token = _authServices.ExtractJWTTokenFromHttpRequest(HttpContext.Request);

            Console.WriteLine("Token Extracted: {0}", token);

            if (string.IsNullOrEmpty(token))
                return;
            
            _authData.AddToken(token, false);

             Console.WriteLine("logout successful. Token invalidated");
        }
    }
}