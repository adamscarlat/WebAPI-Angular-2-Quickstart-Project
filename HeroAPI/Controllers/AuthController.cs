using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HeroAPI.Middleware.TokenMiddleware;
using HeroAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using ViewModels;

//TODO: return redirect from all POST requests (added as a url in json response)
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
        private readonly JWTAuthTokenServices _authServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthData authData, JWTAuthTokenServices authServices, UserManager<ApplicationUser> userManager)
        {
            _authData = authData;
            _authServices = authServices;
            _userManager = userManager;
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

        /// <summary>
        /// Registers a new user. New user must be validated against rules in the NewUserViewModel 
        /// and also have a unique username and email. Once user is registered successfully, a
        /// new JWT token is generated for him 
        /// </summary>
        /// <param name="newUserViewModel">View model for the new user</param>
        /// <returns>JSON object to represent the registration success status</returns>
        [HttpPost]
        [Route("api/auth/register")]
        public async Task<JsonResult> Register([FromBody]NewUserViewModel newUserViewModel)
        {
            bool isUserUnique = await ValidateNewUserUnique(newUserViewModel, ModelState);
            if (ModelState.IsValid && isUserUnique)
            {
                bool isRegisterSucceeded = await RegisterNewUser(newUserViewModel);

                if (isRegisterSucceeded)
                {      
                    var token = await GetJWTToken(newUserViewModel.Username, newUserViewModel.Password);
                    return Json(Utilities.CreateJsonSuccessReponse(
                            new {
                                AccessToken = token,
                                Message = "User successfully created"
                            },
                            "api/Heroes"
                        ));
                }

                ModelState.TryAddModelError("Application", ResourceMaster.GeneralApplicationError);

            }

            var fieldsErrors = Utilities.CreateFieldErrorDictionary(ModelState);
            return Json(Utilities.CreateJsonErrorResponse(fieldsErrors));
        }

        /// <summary>
        /// Gets a JWT token for the user from the token service 
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">user password</param>
        /// <returns>a JWT string</returns>
        private async Task<string> GetJWTToken(string username, string password)
        {

            var formKeyValue = new Dictionary<string, string>();
            formKeyValue.Add("username", username);
            formKeyValue.Add("password", password);

            var uri = "http://localhost:5000" + new TokenProviderOptions().Path;

            string content;
            using (HttpClient httpClient = new HttpClient())
            {
                var formContent = new FormUrlEncodedContent(formKeyValue);
                var httpResponse = await httpClient.PostAsync(uri, formContent);
                content = await httpResponse.Content.ReadAsStringAsync();
            }

            if (!string.IsNullOrEmpty(content))
            {
                try{
                    var responseMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    return responseMap["access_token"];
                }
                catch (Exception ex){
                    System.Console.WriteLine(ex.Message);
                }
            }

            return "Access token not found";

        }

        /// <summary>
        /// Validates a new user to be unique by checking if another user exists with the same
        /// email or username. If so, appropriate field errors are added to the modelstate
        /// /// </summary>
        /// <param name="newuserViewModel">view model for the new user</param>
        /// <param name="modelState">model state of the new user view model</param>
        /// <returns>true if user is unique, false otherwise</returns>
        private async Task<bool> ValidateNewUserUnique(NewUserViewModel newuserViewModel, ModelStateDictionary modelState)
        {   
            bool isUserUnique = true;

            var userByEmail = await _userManager.FindByEmailAsync(newuserViewModel.Email);
            var userByusername = await _userManager.FindByNameAsync(newuserViewModel.Username);

            if (userByEmail != null)
            {
                isUserUnique = false;
                modelState.TryAddModelError("Email", ResourceMaster.InvalidEmailAddressError);
            }

            if (userByusername != null)
            {
                isUserUnique = false;
                modelState.TryAddModelError("Username", ResourceMaster.InvalidUsernameError);
            }

            return isUserUnique;
        }

        /// <summary>
        /// Registers a new user.  
        /// </summary>
        /// <param name="newUserViewModel">new user view model</param>
        /// <returns>true if register action succeeded, false otherwise</returns>
        private async Task<bool> RegisterNewUser(NewUserViewModel newUserViewModel)
        {
            var newUser = new ApplicationUser 
            {
                UserName = newUserViewModel.Username,
                Email = newUserViewModel.Email
            };

            var result = await _userManager.CreateAsync(newUser, newUserViewModel.Password);

            return result.Succeeded;
        }
    }
}