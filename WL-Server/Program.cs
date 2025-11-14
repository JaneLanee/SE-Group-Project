using MySql.Data.MySqlClient;
using WL_Server.Config;
using WL_Server.User;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//REGISTER DEPENDENCIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

//DB?

var testDB = new DBConn();

Console.WriteLine(testDB.IsConnected());

app.MapGet("/", () => "Hello World!");

app.MapControllers();


app.Run();