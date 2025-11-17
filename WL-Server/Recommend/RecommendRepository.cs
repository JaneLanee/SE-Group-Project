using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using WL_Server.Config;

namespace WL_Server.Recommend;

public class RecommendRepository : IRecommendRepository
{
    public Recommend GetMovieById(Recommend movie)
    {
        //INITIALIZE RESULT INFO
        var result = new Recommend();
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        if (db.IsConnected())
        {
            try
            {
                string query = "SELECT * FROM Movies WHERE Movie_Id = @movieId";

                var cmd = new MySqlCommand(query, db.Conn);

                cmd.Parameters.AddWithValue("@movieId", movie.MovieId);

                using var myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    result.MovieId = myReader.GetInt32("Movie_Id");
                    result.MovieTitle = myReader.GetString("title");
                    result.MoviePoster = myReader.GetString("poster_url");
                    result.ReleaseYear = myReader.GetInt32("release_year");
                    result.LastUpdated = myReader.GetDateTime("last_updated");
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        db.Close();
        return result;
    }

    public Recommend[] GetRecommendList(int year)
    {
        //
        var result = new Recommend[8];
        int count = 0;
        Recommend movie = new Recommend();
        
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        if (db.IsConnected())
        {
            try
            {
                string query = "SELECT TOP 8 * FROM Movies WHERE release_year = @release_year";

                var cmd = new MySqlCommand(query, db.Conn);

                cmd.Parameters.AddWithValue("@release_year", year);

                using var myReader = cmd.ExecuteReader();

                while (myReader.Read())
                {
                    result[count].MovieId = myReader.GetInt32("Movie_Id");
                    result[count].MovieTitle = myReader.GetString("title");
                    result[count].MoviePoster = myReader.GetString("poster_url");
                    result[count].ReleaseYear = myReader.GetInt32("release_year");
                    result[count].LastUpdated = myReader.GetDateTime("last_updated");

                    count++;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        db.Close();
        return result;
    }

    public bool AddMovie(Recommend movie)
    {
        //INITIALIZE CONNECTION FOR DB LOGIC
        var db = new DBConn();
        if (db.IsConnected())
        {
            try
            {
                string query = "INSERT INTO Movies (Movie_Id, title, poster_url, release_year, last_updated) VALUES (@movieId, @movieTitle, @moviePoster, @releaseYear, @lastUpdated)";
                
                var cmd = new MySqlCommand(query, db.Conn);
                
                cmd.Parameters.AddWithValue("@movieId", movie.MovieId);
                cmd.Parameters.AddWithValue("@movieTitle", movie.MovieTitle);
                cmd.Parameters.AddWithValue("@moviePoster", movie.MoviePoster);
                cmd.Parameters.AddWithValue("@releaseYear", movie.ReleaseYear);
                cmd.Parameters.AddWithValue("@lastUpdated", movie.LastUpdated);
                
                cmd.ExecuteNonQuery();
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        db.Close();
        return true;
    }
}