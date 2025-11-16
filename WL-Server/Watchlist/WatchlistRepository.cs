using MySql.Data.MySqlClient;
using Org.BouncyCastle.Cms;
using WL_Server.Config;

namespace WL_Server.Watchlist;

public class WatchlistRepository : IWatchlistRepository
{
    //GRAB THE USERS WATCHLIST
    public Watchlist[] GetWatchlist(Watchlist watchlist)
    {
        var results = new Watchlist();
        Watchlist[] userWatchlist = new Watchlist[100];
        // INITIALIZE DB CONNECTION, THEN CHECK IF CONNECTED
        var db = new DBConn();

        if (db.IsConnected())
        {
            //QUERY TO GRAB USER WATCHLIST
            string query = "SELECT * FROM Watchlist WHERE User_Id = @userId";
            var cmd = new MySqlCommand(query, db.Conn);

            cmd.Parameters.AddWithValue("@userId", watchlist.UserId);
            
            //DISPLAY RESULTS
            using var myReader = cmd.ExecuteReader();

            //THIS WILL READ THE USER INFO
            int count = 0;
            
            while (myReader.Read())
            {

                results.UserId = myReader.GetInt32("User_Id");
                results.MovieId = myReader.GetInt32("Movie_Id");
                string watchlistStatus = myReader.GetString("status");
                results.Status = Enum.Parse<Watchlist.StatusType>(watchlistStatus);
                results.PersonalRating = myReader.GetFloat("personal_rating");
                results.DateAdded = myReader.GetDateTime("date_added");

                userWatchlist[count] = results;
                count++;
            }
        }
        
        
        
        
        return userWatchlist;
    }

    //GRAB SPECIFIC MOVIE FROM USER WATCHLIST
    public Watchlist GetWatchlistById(Watchlist watchlist)
    {
        Watchlist result = new Watchlist();
        // INITIALIZE DB CONNECTION, THEN CHECK IF CONNECTED
        var db = new DBConn();

        if (db.IsConnected())
        {
            // QUERY FOR SPECIFIC MOVIE IN WATCHLIST
            string query = "SELECT * FROM Watchlist WHERE User_Id = @userId AND Watchlist_Id = @watchlistId";

            var cmd = new MySqlCommand(query, db.Conn);
            cmd.Parameters.AddWithValue("@userId", watchlist.UserId);
            cmd.Parameters.AddWithValue("@movieId", watchlist.MovieId);

            using var myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                result.UserId = myReader.GetInt32("User_Id");
                result.MovieId = myReader.GetInt32("Movie_Id");
                string watchlistStatus = myReader.GetString("status");
                result.Status = Enum.Parse<Watchlist.StatusType>(watchlistStatus);
                result.PersonalRating = myReader.GetFloat("personal_rating");
                result.DateAdded = myReader.GetDateTime("date_added");
            }


        }

        return result;
    }

    //ADD MOVIE TO WATCHLIST
    public void Create(Watchlist watchlist)
    {
        // INITIALIZE DB CONNECTION, THEN CHECK IF CONNECTED
        var db = new DBConn();

        if (db.IsConnected())
        {
            try
            {
                // QUERY TO ADD MOVIE TO WATCHLIST
                string query =
                    "INSERT INTO Watchlist (Movie_Id, personal_rating, date_added) VALUES (@movieId, @personalRating, @dateAdded)";

                var cmd = new MySqlCommand(query, db.Conn);

                
                cmd.Parameters.AddWithValue("@movieId", watchlist.MovieId);
                cmd.Parameters.AddWithValue("@personalRating", watchlist.PersonalRating);
                cmd.Parameters.AddWithValue("@dateAdded", watchlist.DateAdded);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                //LOG ERROR
                Console.WriteLine(ex.Message);
            }

        }
    }

    //UPDATE STATUS ON MOVIE IN USER WATCHLIST
    public void Update(Watchlist watchlist)
    {
        // INITIALIZE DB CONNECTION, THEN CHECK IF CONNECTED
         var db = new DBConn();

         if (db.IsConnected())
         {
             try
             {
                 // UPDATE QUERY
                 string query = "UPDATE Watchlist SET status = @status WHERE User_Id = @userId AND Movie_Id = @movieId";

                 var cmd = new MySqlCommand(query, db.Conn);

                 cmd.Parameters.AddWithValue("@status", watchlist.Status);
                 cmd.Parameters.AddWithValue("@userId", watchlist.UserId);
                 cmd.Parameters.AddWithValue("@movieId", watchlist.MovieId);

                 cmd.ExecuteNonQuery();
             }
             catch (MySqlException ex) // IN CASE OF ERROR
             {
                 //LOG ERROR
                 Console.WriteLine(ex.Message);
             }

         }
    }

    //REMOVE MOVIE FROM USER WATCHLIST
    public void Delete(Watchlist watchlist)
    {
        // INITIALIZE DB CONNECTION, THEN CHECK IF CONNECTED
        var db = new DBConn();

        if (db.IsConnected())
        {
            try
            {
                // QUERY TO REMOVE MOVIE FROM WATCHLIST
                string query = "REMOVE FROM Watchlist WHERE User_Id = @userId AND Movie_Id = @movieId";

                var cmd = new MySqlCommand(query, db.Conn);

                cmd.Parameters.AddWithValue("@userId", watchlist.UserId);
                cmd.Parameters.AddWithValue("@movieId", watchlist.MovieId);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
            }
        }
    }
}