using System;
using Microsoft.IdentityModel.Tokens;

//Code taken from: https://stormpath.com/blog/token-authentication-asp-net-core
namespace HeroAPI.Middleware.TokenMiddleware
{
    /*
    These options are assined in Startup.cs 
    */
    public class TokenProviderOptions
    {
        //path to obtain a token
        public string Path { get; set; } = "/token";
 
        public string Issuer { get; set; }
 
        public string Audience { get; set; }
        
        //one week expiration
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(10080);
 
        public SigningCredentials SigningCredentials { get; set; }
    }
}