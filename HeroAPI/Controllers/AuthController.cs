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
        public async Task Logout()
        {
            var token = _authServices.ExtractJWTTokenFromHttpRequest(HttpContext.Request);

            if (string.IsNullOrEmpty(token))
                return;
            
            await _authData.AddToken(token, false);
        }

        //Register
        /*
            -Takes in a new user viewmodel object
            -Validates it
            -Creates a new user object and saves
            -Calles the token API, gets the token and returns it to the user
            
            Validation specs
            -Required: username, password
            -Username: required, cannot contain specific characters, must be > 3 letters
         */

         //Login
         /*
            -Takes in a user viewmodel object (or just username and password?)
            -Authenticates them (get logic from the token)
            -If authenticated, call the token and get token

            To consume another API from here see HttpClient library usage

            !issue: /token API is still open and will not validate creds
          */
    }
}