namespace WL_Server.Config;
using MySql.Data;
using MySql.Data.MySqlClient;

//THIS FILE IS FOR CONNECTING THE DOTNET SERVER TO THE DATBASE
public class DBConn
{
    private MySql.Data.MySqlClient.MySqlConnection MyConn;
    //CONNECTION STRING
    private string connStr = "server=127.0.0.1;uid=watchlist_user;pwd=secure_password123;database=watchlist";
    public MySqlConnection Connection { get; set; }
    





}