using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

//Code taken from: https://stormpath.com/blog/token-authentication-asp-net-core
namespace HeroAPI.Middleware.TokenMiddleware
{
    /// <summary>
    /// JWT token provider. Generate token upon receiving login credentials
    /// </summary>
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;

        public TokenProviderMiddleware(RequestDelegate next, IOptions<TokenProviderOptions> options,
        SignInManager<ApplicationUser> signInManager,  UserManager<ApplicationUser> userManager)
        {
            _next = next;
            _options = options.Value;

            _signInManager = signInManager;
            _userManager = userManager;
        }


        /// <summary>
        /// When this class is used as middleware in the asp.net pipeline this method
        /// will get called. If the request is POST and has form content type it will 
        /// forward the request to token generation
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>A task that represents the completion of the operation</returns>
        public Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateToken(context);
        }


        /// <summary>
        ///  Checks if username and password were authenticated and generates the token.
        /// If not return a 400 response
        /// </summary>
        /// <param name="context">Http context</param>
        /// <returns>A task that represents the completion of the operation</returns>
        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
        
            var identity = await GetIdentity(username, password);
            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }
        
            var now = DateTimeOffset.UtcNow;
        
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
        
            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                //notBefore: now.DateTime,
                expires: now.DateTime.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
                
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            
            // Serialize and return the response
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
        
        /// <summary>
        /// Authenticate username and password and create a ClaimsIdentity object 
        /// </summary>
        /// <param name="username">username string</param>
        /// <param name="password">password string</param>
        /// <returns>ClaimsIdentity object for the JWT token</returns>
        public async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            if (username == null || password == null)
                return null;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var isValidLogin= await _userManager.CheckPasswordAsync(user, password);
            if (isValidLogin)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                return new ClaimsIdentity(new GenericIdentity(username, "Token"), claims);
            }

            // Credentials are invalid, or account doesn't exist
            return null;
        }

    }
}
