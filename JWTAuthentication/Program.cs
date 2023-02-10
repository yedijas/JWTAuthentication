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

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.Configure<LiteDBOptions>(builder.Configuration.GetSection(LiteDBOptions.LiteDB));
        builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection(JWTOptions.JWT));
        builder.Services.AddSingleton<ILiteDbContext, DatabaseContext>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IAudienceService, AudienceService>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddTransient<ITokenHelper, TokenHelper>();

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

        // Configure the HTTP request pipeline.
        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
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
            var audienceService = new AudienceService(dbc);
            foreach (string singleAudience in audiences)
            {
                retval = audienceService.CheckHostExists(singleAudience);
                if (retval)
                    break;
            }
        }

        return retval;
    }
}