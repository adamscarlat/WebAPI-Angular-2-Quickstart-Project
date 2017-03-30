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
            Console.WriteLine("in logout controller...");

            bool isValidAuthHeader;
            var token = ExtractTokenFromHeader(out isValidAuthHeader);

            Console.WriteLine("Token Extracted: {0}", token);

            if (!isValidAuthHeader)
                return;
            
            _authData.AddToken(token, false);

             Console.WriteLine("logout successful. Token invalidated");
        }

        private string ExtractTokenFromHeader(out bool isValidAuthHeader)
        {
            isValidAuthHeader = true;
            StringValues authHeader = string.Empty;
            HttpContext.Request.Headers.TryGetValue("Authorization", out authHeader);

            var authHeaderArray = authHeader.ToString().Split(' ');
            if (authHeaderArray.Length > 1)
            {
                if (authHeaderArray[0].Equals("Bearer"))
                    return authHeaderArray[1];
            }
            
            isValidAuthHeader = false;

            Console.WriteLine("Failed to extract token");

            return string.Empty;
        }

    }
}