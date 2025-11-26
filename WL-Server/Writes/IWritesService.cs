namespace WL_Server.Writes;

//ABSTRACT METHODS FOR WRITES SERVICE (BUSINESS LOGIC)
public interface IWritesService
{
    public List<Writes> FetchWritesByMovieId(Writes writes);
    
    public Writes[] FetchWritesByUserId(Writes writes);
    
    public Writes[] FetchAllWrites();

    public bool CreateWrites(Writes writes, string movieTitle);
    
    public bool Upvote(Writes writes);
    
    public bool DeleteWrites(Writes writes);
    
}