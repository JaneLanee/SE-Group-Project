namespace WL_Server.Recommend;

public class Recommend
{
    // MOVIES Movie_Id int
    public int? MovieId { get; set; }
    
    // MOVIES title string
    public string? MovieTitle { get; set; }
    
    // MOVIES poster_url string
    public string? MoviePoster { get; set; }
    
    // MOVIES release_year YEAR
    public int? ReleaseYear { get; set; }
    
    // MOVIES last_updated TIMESTAMP
    public DateTime? LastUpdated { get; set; }
}