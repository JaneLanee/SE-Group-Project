using WL_Server.Other;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WL_Server.User;


//DB LOGIC
public class UserRepository : IRepository<User>
{
    MySqlCommand query = new MySqlCommand();
    
    public void Get(User user)
    {
        query.CommandText = "@SELECT * FROM User WHERE User_Id = @userID";
        query.Parameters.AddWithValue("@userID", user.ID);


    }

    public void Create(User user)
    {
        //CREATE LOGIC
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