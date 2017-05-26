using System.Threading.Tasks;
using HeroAPI.Data.DataProviderInterfaces;
using HeroAPI.Services;
using Microsoft.AspNetCore.Http;

namespace HeroAPI.Middleware.TokenMiddleware
{
    /// <summary>
    /// Middleware to check if incoming token is invalidated. If yes,
    /// prevent http request from going further and return a 401 Unauthorized
    /// </summary>
    public class TokenBlacklistValidationMiddleware
    {
        private IAuthData _authData;
        private RequestDelegate _next;

        public TokenBlacklistValidationMiddleware(RequestDelegate next, IAuthData authData)
        {
            _next = next;
            _authData = authData;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var token  = JWTAuthTokenServices.ExtractJWTTokenFromHttpRequest(context.Request);
            var isTokenValid = false;

            //No token found. Continue pipeline and let other validation decide (controller, token gen, etc...)
            if (string.IsNullOrEmpty(token))
                isTokenValid = true;

            TokenStore tokenStoreEntity;
            //TODO: create method to check cache and if not in cache go to db

            //not found in cache, check db
            tokenStoreEntity = await _authData.GetToken(token);

            //Console.WriteLine("token: {0} \nis valid: {1}", tokenStoreEntity?.Token, tokenStoreEntity?.IsValid);

            //if token is not in db or is valid and not expired- consider token not invalidated
            if (tokenStoreEntity == null || (tokenStoreEntity.IsValid && !JWTAuthTokenServices.IsTokenExpired(tokenStoreEntity.Token)))
                isTokenValid = true;

            if (!isTokenValid)
            {
                //token was found and is invalid 
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("invalid token"); 
                return;   
            }

            await _next(context);
        
        }
    }
}