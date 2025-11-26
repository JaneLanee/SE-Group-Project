namespace WL_Server.Writes;

//ABSTRACT METHODS FOR WRITES REPOSITORY
public interface IWritesRepository
{
    public List<Writes>  GetWritesByMovieId(Writes writes);

    public Writes[] GetWritesByUserId(Writes writes);

    public bool AddMovie(int movieId, string movieTitle);

    public int GetMovieById(int movieId);

    public Writes[] GetWrites();

    public bool Create(Writes writes);

    public bool UpdateUpvoteCounter(Writes writes);

    public bool Delete(Writes writes);
}