namespace WL_Server.Writes;

//DATA MODEL FOR WRITES TABLE IN WATCHLIST SQL DB
public class Writes
{
    //WRITES User_Id int
    public int? UserId { get; set; }
    
    //WRITES Movie_Id int
    public int? MovieId { get; set; }
    
    //WRITES rating decimal
    public float? Rating { get; set; }
    
    //WRITES comment text
    public string? Comment { get; set; }
    
    //WRITES Date_posted DATE
    public DateTime? DatePosted { get; set; }
    
    //WRITES upvote_count int
    public int? UpvoteCount { get; set; } = 0;
}