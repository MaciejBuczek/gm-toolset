using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Identity
{
    public static class JwtAuthenticator
    {
        public static void AddJwtAuthentication(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            var configOptions = configurationManager.GetSection(nameof(JwtAuthenticationOptions)).Get<JwtAuthenticationOptions>()
                ?? throw new ApplicationException("JwtAuthenticationOptions configuration section is missing or invalid.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configOptions.Issuer,
                    ValidAudience = configOptions.Audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configOptions.Key))
                };
            });
        }
    }
}
