using WL_Server.Other;
using MySql.Data;
using MySql.Data.MySqlClient;
using WL_Server.Config;
using System.Linq;
using static WL_Server.Config.DBConn;

namespace WL_Server.User;


//DB LOGIC
public class UserRepository : IUserRepository
{

    //FETCH USER BY THEIR ID
    public User GetById(User user)
    {
        //INITIALIZE RESULT INFO
        var result = new User();
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        if (db.IsConnected())
        {
            //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
            //GET BY ID SQL QUERY
            string query = "SELECT * FROM Users WHERE User_Id = @userId";
            var cmd = new MySqlCommand(query, db.Conn);
            cmd.Parameters.AddWithValue("@userID", user.ID);
            
            //DISPLAY RESULTS
            using var myReader = cmd.ExecuteReader();

            //THIS WILL READ THE USER INFO
            while (myReader.Read())
            {

                result.ID = myReader.GetInt32("User_Id");
                result.username = myReader.GetString("Username");
                result.email = myReader.GetString("Email");
                result.PwHash = myReader.GetString("Password_hash");
            }

        }
        
        //RETURN RESULT OF THIS QUERY
        db.Close();
        return result;
    }
    
    //FETCH USER BY THEIR USERNAME
    public User GetByUsername(User user)
    {
        //INITIALIZE RESULT INFO
        var result = new User();
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        //var query = new MySqlCommand();
        if (db.IsConnected())
        {
            //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
            //GET BY ID SQL QUERY
            
            string query = "SELECT * FROM Users WHERE Username = @username";
            var cmd = new MySqlCommand(query, db.Conn);
            cmd.Parameters.AddWithValue("@username", user.username);
            
            //DISPLAY RESULTS
            using var myReader = cmd.ExecuteReader();

            //THIS WILL READ THE USER INFO
            while (myReader.Read())
            {

                result.ID = myReader.GetInt32("User_Id");
                result.username = myReader.GetString("Username");
                result.email = myReader.GetString("Email");
                result.PwHash = myReader.GetString("Password_hash");
            }

        }
        db.Close();
        return result;
    }
    
    //FETCH USER BY THEIR Email
    public User GetByEmail(User user)
    {
        //INITIALIZE RESULT INFO
        var result = new User();
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        if (db.IsConnected())
        {
            //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
            //GET BY ID SQL QUERY
            string query = "SELECT * FROM Users WHERE Email = @email";
            var cmd = new MySqlCommand(query, db.Conn);
            cmd.Parameters.AddWithValue("@email", user.email);
            
            //DISPLAY RESULTS
            using var myReader = cmd.ExecuteReader();

            //THIS WILL READ THE USER INFO
            while (myReader.Read())
            {

                result.ID = myReader.GetInt32("User_Id");
                result.username = myReader.GetString("Username");
                result.email = myReader.GetString("Email");
                result.PwHash = myReader.GetString("Password_hash");
            }

        }
        db.Close();
        return result;
    }

    public void GetAll(User user)
    {
        var db = new DBConn();
        //CHECK IF DB IS CONNECTED
        if (db.IsConnected())
        {
            
            //WRITE SQL QUERY
            var cmd = new MySqlCommand();
            cmd.CommandText = "SELECT * FROM Users";

            using var myReader = cmd.ExecuteReader();
            {
                while (myReader.Read())
                {
                    var id = myReader.GetInt32("User_Id");
                    var username = myReader.GetString("Username");
                }
            }
            db.Close();
        }
    }

    public void Create(User user)
    {
        //INITIALIZE CONNECTION
        var db = new DBConn();

        //CHECK FOR CONNECTION
        if (db.IsConnected())
        {
            //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
            try
            {
                string query =
                    "INSERT INTO Users (Username, Email, Password_hash, DoB) VALUES (@username, @email, @password, @dateOfBirth)";
                var cmd = new MySqlCommand(query, db.Conn);
                cmd.Parameters.AddWithValue("?username", user.username);
                cmd.Parameters.AddWithValue("?email", user.email);
                cmd.Parameters.AddWithValue("?password", user.PwHash);
                cmd.Parameters.AddWithValue("?dateOfBirth", user.dateOfBirth);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        db.Close();
    }

    public void Update(User user)
    {
        //UPDATE LOGIC
    }

    public void Delete(User user)
    {
        //DELETE LOGIC
    }
}