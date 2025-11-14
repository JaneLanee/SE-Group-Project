namespace WL_Server.Config;
using MySql.Data;
using MySql.Data.MySqlClient;

//THIS FILE IS FOR CONNECTING THE DOTNET SERVER TO THE DATBASE
public class DBConn
{
    //private DBConn() {}
    //CONNECTION STRING
    string _connStr = "server=127.0.0.1;uid=root;pwd=password;database=watchlist";
    public MySqlConnection Conn { get; set; }
    //private var conn =  new MySqlConnection();

    //INSTANCE
    public DBConn()
    {
    }

    //CHECK IF DB IS CONNECTED
    public bool IsConnected()
    {
        try
        {
            //CONNECT TO DB
            Conn = new MySqlConnection(_connStr);
            Conn.Open();
            return true;
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    
    //CLOSE CONNECTION
    public void Close()
    {
        try
        {
            Conn.Close();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    
    





}