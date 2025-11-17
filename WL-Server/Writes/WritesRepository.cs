using MySql.Data.MySqlClient;
using Org.BouncyCastle.Cms;
using WL_Server.Config;

namespace WL_Server.Writes;

public class WritesRepository : IWritesRepository
{
    public Writes[] GetWritesByMovieId(Writes writes)
    {
        // INITIALIZE RESULTS
        Writes[] result = new Writes[50];
        int count = 0;
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        
        // MAKE SURE DB IS CONNECTED
        if (db.IsConnected())
        {
            try
            {
                //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
                // GET BY ID SQL QUERY
                string query = "SELECT * FROM Writes WHERE Movie_Id = @movieId";
                var cmd = new MySqlCommand(query, db.Conn);
                cmd.Parameters.AddWithValue("@movieId", writes.MovieId);

                //READ RESULTS
                using var myReader = cmd.ExecuteReader();

                // STORE RESULTS
                while (myReader.Read())
                {
                    result[count].UserId = myReader.GetInt32("User_Id");
                    result[count].MovieId = myReader.GetInt32("Movie_Id");
                    result[count].Rating = myReader.GetFloat("rating");
                    result[count].Comment = myReader.GetString("comment");
                    result[count].DatePosted = myReader.GetDateTime("date_posted");
                    result[count].UpvoteCount = myReader.GetInt32("upvote_count");

                    count++;

                }
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
            }
        }
        // CLOSE DB AND RETURN RESULT
        db.Close();
        return result;
    }

    public Writes[] GetWritesByUserId(Writes writes)
    {
        // INITIALIZE RESULTS
        Writes[] result = new Writes[50];
        int count = 0;
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        
        // MAKE SURE DB IS CONNECTED
        if (db.IsConnected())
        {
            try
            {
                //INITIALIZE SQL COMMAND IN ORDER TO MAKE QUERY
                // GET BY ID SQL QUERY
                string query = "SELECT * FROM Writes WHERE User_Id = @userId";
                var cmd = new MySqlCommand(query, db.Conn);
                cmd.Parameters.AddWithValue("@userId", writes.UserId);
                
                // READ RESULTS
                using var myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    result[count].UserId = myReader.GetInt32("User_Id");
                    result[count].MovieId = myReader.GetInt32("Movie_Id");
                    result[count].Rating = myReader.GetFloat("rating");
                    result[count].Comment = myReader.GetString("comment");
                    result[count].DatePosted = myReader.GetDateTime("date_posted");
                    result[count].UpvoteCount = myReader.GetInt32("upvote_count");

                    count++;
                }
                
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
            }

        }
        db.Close();
        return result;
    }

    public Writes[] GetWrites()
    {
        // INITIALIZE RESULTS
        Writes[] result = new Writes[50];
        int count = 0;
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        
        // MAKE SURE DB IS CONNECTED
        if (db.IsConnected())
        {
            try
            {
                string query = "SELECT * FROM Writes";
                var cmd = new MySqlCommand(query, db.Conn);

                using var myReader = cmd.ExecuteReader();

                // READ RESULTS AND THEN STORE THEM
                while (myReader.Read())
                {
                    result[count].UserId = myReader.GetInt32("User_Id");
                    result[count].MovieId = myReader.GetInt32("Movie_Id");
                    result[count].Rating = myReader.GetFloat("rating");
                    result[count].Comment = myReader.GetString("comment");
                    result[count].DatePosted = myReader.GetDateTime("date_posted");
                    result[count].UpvoteCount = myReader.GetInt32("upvote_count");

                    count++;
                }
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
            }
        }
        // CLOSE DB
        db.Close();
        return result;
    }

    public bool Create(Writes writes)
    {
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        
        // MAKE SURE DB IS CONNECTED
        if (db.IsConnected())
        {
            try
            {
                // QUERY TO CREATE WRITES
                string query =
                    "INSERT INTO Writes (User_ID, Movie_Id, rating, comment, date_posted, upvote_count) VALUES (@userId, movieId, @rating, @comment, @datePosted, @upvoteCount)";
                var cmd = new MySqlCommand(query, db.Conn);

                //ASSIGN VALUES IN QUERY
                cmd.Parameters.AddWithValue("@userId", writes.UserId);
                cmd.Parameters.AddWithValue("@movieId", writes.MovieId);
                cmd.Parameters.AddWithValue("rating", writes.Rating);
                cmd.Parameters.AddWithValue("@comment", writes.Comment);
                cmd.Parameters.AddWithValue("@datePosted", writes.DatePosted);
                cmd.Parameters.AddWithValue("@upvoteCount", writes.UpvoteCount);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        // CLOSE DB AND RETURN TRUE
        db.Close();
        return true;
    }

    public bool UpdateUpvoteCounter(Writes writes)
    {
        // 
        var db = new DBConn();
        
        //
        if (db.IsConnected())
        {
            try
            {
                // QUERY TO UPDATE UPVOTE COUNT
                string query =
                    "UPDATE Writes SET UpvoteCount = @upvoteCount WHERE User_Id = @userId AND Movie_Id = @movieId";

                var cmd = new MySqlCommand(query, db.Conn);

                writes.UpvoteCount = writes.UpvoteCount + 1;

                cmd.Parameters.AddWithValue("@upvoteCount", writes.UpvoteCount);
                cmd.Parameters.AddWithValue("@userId", writes.UserId);
                cmd.Parameters.AddWithValue("@movieId", writes.MovieId);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        //CLOSE DB AND RETURN TRUE
        db.Close();
        return true;
    }

    public bool Delete(Writes writes)
    {
        //INITIALIZE CONNECTION FOR DB LOGIC
         var db = new DBConn();
         
        // // MAKE SURE DB IS CONNECTED
        if (db.IsConnected())
        {
            try
            {
                // QUERY TO REMOVE WRITES
                string query = "DELETE FROM Writes WHERE User_Id = @userId AND Movie_Id = @movieId";

                var cmd = new MySqlCommand(query, db.Conn);

                // ASSIGN TO VALUES IN QUERY STRING
                cmd.Parameters.AddWithValue("@userId", writes.UserId);
                cmd.Parameters.AddWithValue("@movieId", writes.MovieId);

                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex) // IN CASE OF ERROR
            {
                // LOG ERROR
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        // CLOSE DB AND RETURN TRUE
        db.Close();
        return true;
    }
}