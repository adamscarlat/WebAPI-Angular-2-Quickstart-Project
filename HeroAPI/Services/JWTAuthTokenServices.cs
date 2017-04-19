using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace HeroAPI.Services
{
    /// <summary>
    /// Services for JWT related actions.
    /// </summary>
    public class JWTAuthTokenServices
    {
        /// <summary>
        /// Gets the JWT token from an http request
        /// </summary>
        /// <param name="request">Http request object</param>
        /// <returns>JWT token as string</returns>
        public string ExtractJWTTokenFromHttpRequest(HttpRequest request)
        {
            StringValues authHeader = string.Empty;
            request.Headers.TryGetValue("Authorization", out authHeader);

            var authHeaderArray = authHeader.ToString().Split(' ');
            if (authHeaderArray.Length > 1)
            {
                if (authHeaderArray[0].Equals("Bearer"))
                    return authHeaderArray[1];
            }
            
            Console.WriteLine("Failed to extract token");

            return string.Empty;
        }

        /// <summary>
        /// Checks if the JWT has expired
        /// </summary>
        /// <param name="token">JWT as string</param>
        /// <returns>true if token is expired, false otherwise</returns>
        public bool IsTokenExpired(string token)
        {
            var tokenExpirationTime = GetTokenExpirationDateTime(token);
            var currentTimestamp = (ulong) (DateTime.UtcNow - new DateTime(1970,1,1, 0,0,0)).TotalSeconds;

            return currentTimestamp > tokenExpirationTime;
        }

        /// <summary>
        /// Gets the token expiration datetime 
        /// </summary>
        /// <param name="token">JWT as string</param>
        /// <returns>the expiration date time in epoch format as ulong</returns>
        public ulong GetTokenExpirationDateTime(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;

            var payloadBytes = Convert.FromBase64String(token.Split('.')[1] + "=");
            var payloadStr = Encoding.UTF8.GetString(payloadBytes, 0, payloadBytes.Length);

            //extract the Exp proprty from deserialized token token 
            var exp = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL }).Exp;
            
            return exp;
        }
    }
}