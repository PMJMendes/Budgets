using Krypton.Budgets.API._Hosting;
using Krypton.Budgets.API._Impl;
using Krypton.Budgets.Domain._Ports.Hosting;
using Krypton.Budgets.Domain._Ports.Persistence;
using Krypton.Budgets.Domain._Ports.Security;
using Krypton.Budgets.Logging.Hosting;
using Krypton.Budgets.Persistence.Hosting;
using Krypton.Budgets.Security.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security;
using System.Text.Json;
using IHost = Krypton.Budgets.Domain._Ports.Hosting.IHost;

IdentityModelEventSource.ShowPII = true;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.
    AddHttpContextAccessor().
    AddScoped<IHost, HostImpl>().
    AddBudgetsDomain().
    AddBudgetsPersistence().
    AddBudgetsSecurity().
    AddBudgetsLogging(builder.Configuration.GetSection("Logging"));

builder.Services.
    AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).
    AddJwtBearer(o =>
    {
        var opt = new SecurityOptions();

        o.Authority = builder.Configuration["Security:Jwt:Authority"];
        o.Audience = builder.Configuration["Security:Jwt:Audience"];
        o.MetadataAddress = builder.Configuration["Security:Jwt:MetadataURL"] ?? "";
        o.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidAudiences = new[] { "account", config["Security:Jwt:Audience"] },
            ValidateAudience = false,
            NameClaimType = "preferred_username"
        };

        //if (builder.Environment.IsDevelopment())
        //{
        o.RequireHttpsMetadata = false;
        //}
    });

builder.Services.
    AddAuthorization(options =>
    {
        foreach (var p in Enum.GetValues<SecurityLevel>())
        {
            switch (p)
            {
                case SecurityLevel.Admin:
                    options.AddPolicy("Admin", policy => policy.RequireAssertion(c =>
                    {
                        return JsonSerializer.
                            Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst(claim => claim.Type == "realm_access")?.Value ?? "{}")?.
                            FirstOrDefault().Value?.Any(v => v == "admin") ?? false;
                    }));
                    break;

                case SecurityLevel.Producer:
                    options.AddPolicy("Producer", policy => policy.RequireAssertion(c =>
                    {
                        return JsonSerializer.
                            Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst(claim => claim.Type == "realm_access")?.Value ?? "{}")?.
                            FirstOrDefault().Value?.Any(v => v == "producer") ?? false;
                    }));
                    break;

                case SecurityLevel.Accounting:
                    options.AddPolicy("Accounting", policy => policy.RequireAssertion(c =>
                    {
                        return JsonSerializer.
                            Deserialize<Dictionary<string, string[]>>(c.User?.FindFirst(claim => claim.Type == "realm_access")?.Value ?? "{}")?.
                            FirstOrDefault().Value?.Any(v => v == "accounting") ?? false;
                    }));
                    break;

                case SecurityLevel.Assistant:
                    options.AddPolicy("Assistant", policy => policy.RequireAuthenticatedUser());
                    break;

                default:
                    throw new SecurityException("Unimplemented security policy: " + p);
            }
        }
    });

builder.Services.
    AddCors(o =>
    {
        o.AddPolicy("JiffyCORS", p =>
        {
            var config = builder.Configuration.GetSection("Hosting");

            if (config?["AllowedOrigins"] is string origins)
            {
                p.WithOrigins(origins.Split(';'));
            }

            if (config?["AllowedHeaders"] is string headers)
            {
                p.WithHeaders(headers.Split(';'));
            }

            if (config?["AllowedMethods"] is string methods)
            {
                p.WithMethods(methods.Split(';'));
            }
        });
    });

//if (builder.Environment.IsDevelopment())
//{
builder.Services.
    AddEndpointsApiExplorer().
    AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Budgets API", Version = "v1" });
        c.DocumentFilter<SwaggerOrderFilter>();
        c.UseInlineDefinitionsForEnums();

        c.ConfigureSwaggerSecurity(c =>
        {
            c.AuthorizationURL = builder.Configuration["Security:Swagger:AuthorizationURL"];
            c.TokenURL = builder.Configuration["Security:Swagger:TokenURL"];
        });
    });
//}

builder.Services.AddEndpoints();

var app = builder.Build();

app.Services.GetRequiredService<IPersistenceHosting>().RunMigrations();

//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budgets API v1");

    c.ConfigureSwaggerOAuth(c =>
    {
        c.Audience = builder.Configuration["Security:Jwt:Audience"];
    });
});
//}

app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseCors("JiffyCORS");

app.UseEndpoints(app.Services.GetRequiredService<IDomainExplorer>());

app.Run();

public partial class Program { }
