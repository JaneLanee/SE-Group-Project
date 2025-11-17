using MySql.Data.MySqlClient;
using WL_Server.Config;
using WL_Server.User;
using WL_Server.Watchlist;
using WL_Server.Writes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


//SESSION 
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

var app = builder.Build();

//DB?

var testDB = new DBConn();

Console.WriteLine(testDB.IsConnected());

app.MapGet("/", () => "Hello World!");

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.Run();