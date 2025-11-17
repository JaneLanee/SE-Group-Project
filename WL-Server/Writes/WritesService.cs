using WL_Server.Watchlist;

namespace WL_Server.Writes;

public class WritesService : IWritesService
{
    // IMPLEMENT WRITES REPOSITORY TO INCORPORATE DB LOGIC
    private readonly IWritesRepository _writesRepository;
    private readonly IWatchlistRepository _watchlistRepository;

    public WritesService(IWritesRepository writesRepository, IWatchlistRepository watchlistRepository)
    {
        _writesRepository = writesRepository;
        _watchlistRepository = watchlistRepository;
    }
    
    // GRAB WRITES FOR MOVIE
    public Writes[] FetchWritesByMovieId(Writes writes)
    {
        
        // GRAB FETCH METHOD FROM WRITES REPOSITORY
        // CHECK TO SEE IF WRITES EXISTS
        var writesList = _writesRepository.GetWritesByMovieId(writes);

        
        if (writesList.Length == 0)
        {
            //EMPTY, RETURN NULL
            return null;
        }

        //RETURN WRITES LISTS
        return writesList;
    }

    // GRAB WRITES FOR USER
    public Writes[] FetchWritesByUserId(Writes writes)
    {
        // GRAB FETCH METHOD FROM WRITES REPOSITORY
        // CHECK TO SEE IF WRITES EXISTS
        var writesList = _writesRepository.GetWritesByUserId(writes);

        
        if (writesList.Length == 0)
        {
            //EMPTY, RETURN NULL
            return null;
        }

        //RETURN WRITES LISTS
        return writesList;
    }

    // GRAB ALL WRITES
    public Writes[] FetchAllWrites()
    {
        // GRAB FETCH METHOD FROM WRITES REPOSITORY
        // CHECK TO SEE IF WRITES EXISTS
        var writesList = _writesRepository.GetWrites();

        
        if (writesList.Length == 0)
        {
            //EMPTY, RETURN NULL
            return null;
        }

        //RETURN WRITES LISTS
        return writesList;
    }

    // CREATE A WRITES FOR MOVIE
    public bool CreateWrites(Writes writes)
    {
        if (writes.UserId == null)
        {
            return false;
        }
        
        //CREATE WRITES
        _writesRepository.Create(writes);
        return true;

    }
    
    // INCREASE UPVOTE COUNTER
    public bool Upvote(Writes writes)
    {
        //GRAB WRITES
        return _writesRepository.UpdateUpvoteCounter(writes);
    }

    // DELETE WRITES
    public bool DeleteWrites(Writes writes)
    {
        //DELETE WRITES
        //_writesRepository.Delete(writes);
        return _writesRepository.Delete(writes);
    }
}