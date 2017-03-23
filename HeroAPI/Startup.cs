using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HeroAPI.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using HeroAPI.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

using Microsoft.Extensions.Options;
using HeroAPI.Middleware.TokenMiddleware;

namespace HeroAPI
{
    public class Startup
    {
        //allowed client origin domain for CORS
        private readonly string _clientOrigin = "http://localhost:3000";
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI to hero repository
            services.AddScoped<IHeroData, SqliteHeroData>();

            //configure sqlite as default db
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            
            //configure Identity Framework
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //Support for CORS requests
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins(_clientOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());       
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // // global policy - assign here or on each controller
            // app.UseCors("CorsPolicy");
            
            ConfigureToken(app);
            
            app.UseMvc();

            DbSeeder.SeedDbWithSampleUsers(app.ApplicationServices);
        }

        /*
        Configure JWT token and add TokenProviderMiddleware to pipeline
        */
        private void ConfigureToken(IApplicationBuilder app)
        {
            //Code taken from: https://stormpath.com/blog/token-authentication-asp-net-core
            var secretKey = "Rg3&2e!xIo9yHqp<";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            app.UseJwtBearerAuthentication(new JwtBearerOptions
                {
                    AutomaticAuthenticate = true,
                    AutomaticChallenge = true,
                    TokenValidationParameters = GetTokenValidationParameters(signingKey)
                }
            );

            var options = new Middleware.TokenMiddleware.TokenProviderOptions
            {
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
            };

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(options));
        }

        /*
        With this middleware added to your application pipeline, 
        any routes protected with [Authorize] will require a JWT that 
        passes the following validation requirements
        */
        private TokenValidationParameters GetTokenValidationParameters(SymmetricSecurityKey signingKey)
        {
            //Code taken from: https://stormpath.com/blog/token-authentication-asp-net-core
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
            
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",
            
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",
            
                // Validate the token expiry
                ValidateLifetime = true,
            
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            return tokenValidationParameters;
        }
    }
}
