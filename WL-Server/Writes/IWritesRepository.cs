namespace WL_Server.Writes;

//ABSTRACT METHODS FOR WRITES REPOSITORY
public interface IWritesRepository
{
    public Writes[] GetWritesByMovieId(Writes writes);

    public Writes[] GetWritesByUserId(Writes writes);

    public Writes[] GetWrites();

    public bool Create(Writes writes);

    public bool UpdateUpvoteCounter(Writes writes);

    public bool Delete(Writes writes);
}