using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace HeroAPI.Services
{
    /// <summary>
    /// Services related to JWT token 
    /// </summary>
    public class JWTAuthTokenServices
    {
        /// <summary>
        /// Extract the JWT token from the request and return it as a string
        /// </summary>
        /// <param name="request">Incoming http request</param>
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
        /// Decoding the JWT and checking if it's expired by comparing it to the
        /// current epoch time
        /// </summary>
        /// <param name="token">Token to be tested</param>
        /// <returns>True if token is expired</returns>
        public bool IsTokenExpired(string token)
        {
            var tokenExpirationTime = GetTokenExpirationDateTime(token);
            var currentTimestamp = (ulong) (DateTime.UtcNow - new DateTime(1970,1,1, 0,0,0)).TotalSeconds;

            return currentTimestamp > tokenExpirationTime;
        }

        /// <summary>
        /// Decodes the JWT and extracts its expiration time as epoch time
        /// </summary>
        /// <param name="token">Token to be tested</param>
        /// <returns>expiration time in seconds</returns>
        public ulong GetTokenExpirationDateTime(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;

            var payloadBytes = Convert.FromBase64String(token.Split('.')[1] + "=");
            var payloadStr = Encoding.UTF8.GetString(payloadBytes, 0, payloadBytes.Length);

            Console.WriteLine("Exp string: {0}", payloadStr);

            //extract the Exp proprty from deserialized token token 
            var exp = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL }).Exp;
            
            Console.WriteLine("Exp: {0}", exp);
            
            return exp;
        }
    }
}