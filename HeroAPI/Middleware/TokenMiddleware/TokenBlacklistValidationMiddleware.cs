using System;
using System.Threading.Tasks;
using HeroAPI.Services;
using Microsoft.AspNetCore.Http;

namespace HeroAPI.Middleware.TokenMiddleware
{
    public class TokenBlacklistValidationMiddleware
    {
        private IAuthData _authData;
        private RequestDelegate _next;
        private JWTAuthTokenServices _tokenService;

        public TokenBlacklistValidationMiddleware(RequestDelegate next, IAuthData authData, 
        JWTAuthTokenServices tokenServices)
        {
            _next = next;
            _authData = authData;
            _tokenService = tokenServices;
        }

        public Task Invoke(HttpContext context)
        {
            //Todo: check if token is in blacklist and is marked invalid
                //1. check in cache 
                //2. if not in cache check in db
                //3. if found and marked invalid stop the request pipeline
                // else, continue in request pipeline
            
            Console.WriteLine("In TokenBlacklistValidationMiddleware middleware...");

            var token  = _tokenService.ExtractJWTTokenFromHttpRequest(context.Request);

            //No token found. Continue pipeline and let other validation decide (controller, token gen, etc...)
            if (string.IsNullOrEmpty(token))
                return _next(context);

            TokenStore tokenStoreEntity;
            //TODO: create method to check cache and if not in cache go to db

            //not found in cache, check db
            tokenStoreEntity = _authData.GetToken(token);

            Console.WriteLine("token: {0} \nis valid: {1}", tokenStoreEntity?.Token, tokenStoreEntity?.IsValid);

            //if token is not in db or is valid and not expired- consider token not invalidated
            if (tokenStoreEntity == null || (tokenStoreEntity.IsValid && !_tokenService.IsTokenExpired(tokenStoreEntity.Token)))
                return _next(context);

            //token was found and is invalid 
            context.Response.StatusCode = 400;
            return context.Response.WriteAsync("Bad request.");            
        }
    }
}