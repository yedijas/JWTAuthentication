using JWTAuthentication.Databases;
using JWTAuthentication.Databases.Employees;
using JWTAuthentication.Databases.Users;
using JWTAuthentication.Options;

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
        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}