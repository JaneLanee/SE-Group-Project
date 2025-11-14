namespace WL_Server.Writes;

//ABSTRACT METHODS FOR WRITES REPOSITORY
public interface IWritesRepository
{
    public Writes GetWritesByMovieId(Writes writes);

    public Writes GetWritesByUserId(Writes writes);

    public Writes GetWrites(Writes writes);

    public void Create(Writes writes);

    public void Update(Writes writes);

    public void Delete(Writes writes);
}