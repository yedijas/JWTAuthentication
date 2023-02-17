using JWTAuthentication.Databases;
using JWTAuthentication.Databases.Audiences;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Helper;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JWTAuthentication.Databases.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Certificate;
using JWTAuthentication.Databases.Certificates;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.Configure<LiteDBOptions>(builder.Configuration.GetSection(LiteDBOptions.LiteDB));
        builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(JWTOptions.JWT));
        builder.Services.Configure<CertificateOptions>(builder.Configuration.GetSection(CertificateOptions.CERTS));
        builder.Services.AddSingleton<ILiteDbContext, DatabaseContext>();
        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddSingleton<ICertificateService, CertificateService>();
        builder.Services.AddTransient<IAudienceService, AudienceService>();
        builder.Services.AddSingleton<ITokenHelper, TokenHelper>();
        builder.Services.AddSingleton<ICertificateHelper, CertificateHelper>();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            var signinigkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenInfo.HashThisString(builder.Configuration["Jwt:SecretKey"])));
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = signinigkey,
                AudienceValidator = ValidateAudience
            };
        });
        builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate(options => {
            options.AllowedCertificateTypes = CertificateTypes.All; // change as needed
            options.Events = new CertificateAuthenticationEvents
            {
                OnCertificateValidated = context => {
                    
                    var validationService = context.HttpContext.RequestServices.GetService<ICertificateHelper>();
                    if (validationService.ValidateCertificate(context.ClientCertificate, context.HttpContext.Request.Headers.Host))
                    {
                        context.Success();
                    }
                    else
                    {
                        context.Fail("Invalid certificate");
                    }
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context => {
                    context.Fail("Invalid certificate");
                    return Task.CompletedTask;
                }
            };
        });

        // Configure the HTTP request pipeline.
        var app = builder.Build();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
        // comment line below if you dont want to use this on test mode
        InitiateTestData();
    }

    private static void InitiateTestData()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        // will only do shit if the database is not exists yet
        if (!Directory.Exists(config.GetValue<string>("LiteDB:DatabaseDirectory")) && 
            !File.Exists(config.GetValue<string>("LiteDB:DatabaseLocation"))) 
        {
            using (var dbc = new DatabaseContext(config.GetValue<string>("LiteDB:DatabaseLocation")))
            {
                using (var userService = new UserService(dbc))
                {
                    userService.Insert(new User()
                    {
                        UserName = "test",
                        UserPassword = "password",
                        UserEmail = "a.b@c.com",
                        CreatedAt = DateTime.Now
                    });
                }
                using (var audienceService = new AudienceService(dbc))
                {
                    audienceService.Insert(new TokenAudience()
                    {
                        AudienceID = 1,
                        Hostname = "localhost",
                        SystemName = "test"
                    });
                }
            }
        }
    }

    /// <summary>
    /// Validates the audience against the list in database.
    /// This part was supposed to be in the helper class as static method. But often times the database has not initialized, and the services is not 
    /// built yet. So here I am lazily make things work. Please let me know if there is any better way to access the database on this level of startup.
    /// </summary>
    /// <param name="audiences">audiences from the header</param>
    /// <param name="securityToken">security token</param>
    /// <param name="validationParameters">validation parameters</param>
    /// <returns>whether the audience is valid or not</returns>
    private static bool ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
    {
        bool retval = false;
        var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
        string dbloc = config.GetValue<string>("LiteDB:DatabaseLocation");
        using (var dbc = new DatabaseContext(dbloc)) // part where I init the database context.
        {
            using (var audienceService = new AudienceService(dbc))
            {
                foreach (string singleAudience in audiences)
                {
                    retval = audienceService.CheckHostExists(singleAudience);
                    if (retval)
                        break;
                }
            }
        }
        return retval;
    }
}