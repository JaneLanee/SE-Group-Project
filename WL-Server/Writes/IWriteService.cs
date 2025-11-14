namespace WL_Server.Writes;

//ABSTRACT METHODS FOR WRITES SERVICE (BUSINESS LOGIC)
public interface IWriteService
{
    public Writes FetchWritesByMovieId(Writes writes);
    
    public Writes FetchWritesByUserId(Writes writes);
    
    public Writes FetchAllWrites(Writes writes);
    
    public void CreateWrites(Writes writes);
    
    public void UpdateRating(Writes writes);
    
    public void Upvote(Writes writes);
    
    public void DeleteWrites(Writes writes);
    
}