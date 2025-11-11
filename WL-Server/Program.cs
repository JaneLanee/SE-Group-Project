using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//DB?

string connStr = "server=127.0.0.1;uid=watchlist_user;pwd=secure_password123;database=watchlist";

try
{
    var conn = new MySqlConnection(connStr);

    //OPEN CONNECTION
    conn.Open();
}
catch (MySqlException ex)
{
    Console.WriteLine(ex.Message);
}

app.MapGet("/", () => "Hello World!");


app.Run();