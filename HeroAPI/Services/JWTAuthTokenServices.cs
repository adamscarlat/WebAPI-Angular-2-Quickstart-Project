using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace HeroAPI.Services
{
    public class JWTAuthTokenServices
    {
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

        public bool IsTokenExpired(string token)
        {
            var tokenExpirationTime = GetTokenExpirationDateTime(token);
            var currentTimestamp = (ulong) (DateTime.UtcNow - new DateTime(1970,1,1, 0,0,0)).TotalSeconds;

            return currentTimestamp > tokenExpirationTime;
        }

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