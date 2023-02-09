using JWTAuthentication.Databases;
using JWTAuthentication.Databases.Employees;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Helper;
using JWTAuthentication.Models;
using JWTAuthentication.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            var signinigkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenInfo.HashThisString(builder.Configuration["Jwt:SecretKey"])));
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true, //this will be changed to valid hosts in DB
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience= builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = signinigkey,
                AudienceValidator = TokenHelper.ValidateAudience
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
}