namespace WL_Server.Watchlist;

public class Watchlist
{

    public enum StatusType
    {
        want_to_watch,
        watching,
        completed
    }
    
    //WATCHLIST User_Id int
    public int? UserId { get; set; }
    
    //WATCHLIST Movie_Id string
    public int? MovieId { get; set; }
    
    //WATCHLIST status enum STATUS
    public StatusType? Status { get; set; }
    
    //WATCHLIST personal_rating decimal
    public float? PersonalRating { get; set; }
    
    //WATCHLIST date_added
    public DateTime? DateAdded { get; set; }

}