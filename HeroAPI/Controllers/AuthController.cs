using System.Threading.Tasks;
using HeroAPI.Data.DataProviderInterfaces;
using HeroAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ViewModels;

//TODO: change logout to POST 
//TODO: Add integration to Login

namespace HeroAPI.Controllers
{
    /// <summary>
    /// All authentication related action that require an API endpoint
    /// </summary>
    public class AuthController : Controller
    {
        private readonly IAuthData _authData;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthData authData, UserManager<ApplicationUser> userManager)
        {
            _authData = authData;
            _userManager = userManager;
        }

        /// <summary>
        /// Logs in a registered user by communicationg with the token server.
        /// </summary>
        /// <param name="userViewModel">registered user's credentials</param>
        /// <returns>A success response with a access token if login succeeded</returns>
        [Route("api/auth/login")]
        public async Task<IActionResult> Login(NewUserViewModel userViewModel)
        {

            if (userViewModel == null)
                return BadRequest("Enter valid user credentials");

            var token = await JWTAuthTokenServices.GetJWTToken(userViewModel.Username, userViewModel.Password);
            var responseObject = new {
                accessToken = token
            };
            var redirectUrl = "api/heroes";
           
            var successResponse = Utilities.CreateJsonSuccessReponse(responseObject, redirectUrl);
            
            return successResponse;
        }

        /// <summary>
        /// Logout user. Invalidate token by adding it to a blacklist. Next time 
        /// a user tries to use that token he will be rejected.
        /// </summary>
        [Route("api/auth/logout")]
        public async Task Logout()
        {
            var token = JWTAuthTokenServices.ExtractJWTTokenFromHttpRequest(HttpContext.Request);

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
        public async Task<IActionResult> Register([FromBody]NewUserViewModel newUserViewModel)
        {
            bool isUserUnique = await ValidateNewUserUnique(newUserViewModel, ModelState);
            if (ModelState.IsValid && isUserUnique)
            {
                bool isRegisterSucceeded = await RegisterNewUser(newUserViewModel);

                if (isRegisterSucceeded)
                    return RedirectToAction("Login", newUserViewModel);

                ModelState.TryAddModelError("Application", ResourceMaster.GeneralApplicationError);

            }

            var fieldsErrors = Utilities.CreateFieldErrorDictionary(ModelState);
            return Json(Utilities.CreateJsonErrorResponse(fieldsErrors));
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