using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using MySql.Data.MySqlClient;
using WL_Server.Config;
using WL_Server.User;
using WL_Server.Watchlist;
using WL_Server.Writes;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//ENABLE CORS TO ALLOW DATA FROM FRONTEND
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3000/login", "http://localhost:3000/signup")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

//SESSION 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication("CustomCookieOk")
    .AddCookie("CustomCookieOk", options =>
    {
        options.LoginPath = "/api/user/login";
        options.Cookie.Name = "CustomCookieOk";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireClaim("LoggedUser", ClaimTypes.NameIdentifier));
});

// Create the directory
var keysDir = Path.Combine(Directory.GetCurrentDirectory(), "keys");
if (!Directory.Exists(keysDir))
{
    Directory.CreateDirectory(keysDir);
}

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

//REGISTER DEPENDENCIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();
builder.Services.AddScoped<IWatchlistService, WatchlistService>();

builder.Services.AddScoped<IWritesRepository, WritesRepository>();
builder.Services.AddScoped<IWritesService, WritesService>();

//builder.Services.AddScoped<ISession, HttpContext>();

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

//DB?

var testDB = new DBConn();

Console.WriteLine(testDB.IsConnected());

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapGet("/", () => "Hello World!");
app.MapControllers();


app.Run();