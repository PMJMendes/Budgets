using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Security.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IdentityModel.Tokens.Jwt;

namespace Krypton.Budgets.Security.Hosting;

public static class SecurityCompositionStatic
{
    public static IServiceCollection AddBudgetsSecurity(this IServiceCollection services)
    {
        services.AddScoped<ISecurity, SecurityImpl>();

        services.AddSingleton<IUserManagement, KeycloakBridge>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        return services;
    }

    public static void ConfigureSwaggerSecurity(this SwaggerGenOptions options, Action<SwaggerOptions> setOptions)
    {
        var opt = new SwaggerOptions();
        setOptions(opt);

        options.AddSecurityDefinition("pkce", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,

            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(opt.AuthorizationURL ?? "", UriKind.Absolute),
                    TokenUrl = new Uri(opt.TokenURL ?? "", UriKind.Absolute)
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "pkce" }
                },
                Array.Empty<string>()
            }
        });
    }

    public static void ConfigureSwaggerOAuth(this SwaggerUIOptions options, Action<SwaggerOptions> setOptions)
    {
        var opt = new SwaggerOptions();
        setOptions(opt);

        options.OAuthConfigObject = new OAuthConfigObject
        {
            ClientId = opt.Audience,
            UsePkceWithAuthorizationCodeGrant = true
        };
    }
}
