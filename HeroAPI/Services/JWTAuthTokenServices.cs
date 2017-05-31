using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HeroAPI.Middleware.TokenMiddleware;
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
        /// Extract the JWT token from the request and return it as a string
        /// </summary>
        /// <param name="request">Incoming http request</param>
        /// <returns>JWT token as string</returns>
        public static string ExtractJWTTokenFromHttpRequest(HttpRequest request)
        {
            StringValues authHeader = string.Empty;
            request.Headers.TryGetValue("Authorization", out authHeader);

            var authHeaderArray = authHeader.ToString().Split(' ');
            if (authHeaderArray.Length > 1)
            {
                if (authHeaderArray[0].Equals("Bearer"))
                    return authHeaderArray[1];
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Decoding the JWT and checking if it's expired by comparing it to the
        /// current epoch time
        /// </summary>
        /// <param name="token">Token to be tested</param>
        /// <returns>True if token is expired</returns>
        public static bool IsTokenExpired(string token)
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
        public static ulong GetTokenExpirationDateTime(string token)
        {
            if (string.IsNullOrEmpty(token))
                return 0;

            var payloadBytes = Convert.FromBase64String(token.Split('.')[1] + "=");
            var payloadStr = Encoding.UTF8.GetString(payloadBytes, 0, payloadBytes.Length);

            //extract the Exp proprty from deserialized token token 
            var exp = JsonConvert.DeserializeAnonymousType(payloadStr, new { Exp = 0UL }).Exp;
            
            return exp;
        }

                /// <summary>
        /// Gets a JWT token for the user from the token service 
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">user password</param>
        /// <returns>a JWT string</returns>
        public static async Task<string> GetJWTToken(string username, string password)
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

            return string.Empty;

        }
    }
}