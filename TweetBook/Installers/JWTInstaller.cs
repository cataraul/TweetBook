using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TweetBook.Authorization;
using TweetBook.Options;

namespace TweetBook.Installers
{
    public class JWTInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key: Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            builder.Services.AddSingleton(tokenValidationParameters);

            builder.Services.AddAuthentication(configureOptions: x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustWorkForRaul", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement("rauls.com"));
                });
            });

            builder.Services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
        }
    }
}
