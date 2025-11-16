namespace WL_Server.Watchlist;

public class WatchlistService : IWatchlistService
{
    //GET WATCHLIST REPOSITORY
    private IWatchlistRepository _watchlistRepository;

    public WatchlistService(IWatchlistRepository watchlistRepository)
    {
        _watchlistRepository = watchlistRepository;
    }

    //GRAB ALL MOVIES IN USERS WATCHLIST
    public Watchlist[] GetUserWatchlist(Watchlist watchlist)
    {
        //CHECK TO MAKE SURE WATCHLIST EXIST
        var userWatchlist = _watchlistRepository.GetWatchlist(watchlist);

        if (userWatchlist.Length == 0)
        {
            // WATCHLIST IS EMPTY/ DOES NOT EXIST
            return null;
        }

        // RETURN AN ARRAY OF THE USERS WATCHLIST MOVIES
        return userWatchlist;
    }

    // GRAB SPECIFIC MOVIE IN USERS WATCHLIST
    public Watchlist GetMovieFromWatchlist(Watchlist watchlist)
    {
        // CHECK IF WATCHLISTED MOVIE EXISTS IN USERS LIST
        var watchlistMovie = _watchlistRepository.GetWatchlistById(watchlist);
        if (watchlistMovie == null)
        {
            // WATCHILISTED MOVIE DOES NOT EXIST
            return null;
        }

        //RETURN WATCHLISTED MOVIE INFO
        return watchlistMovie;

    }

    //ADD MOVIE TO USER WATCHLIST
    public bool AddToWatchlist(Watchlist watchlist)
    {
        //CREATE NEW 
        if (watchlist.UserId == null)
        {
            return false;
        }
        
        //ADD MOVIE
        _watchlistRepository.Create(watchlist);
        return true;

    }

    // UPDATE THE STATUS OF THE MOVIE IN THE USERS WATCHLIST
    public bool UpdateStatus(Watchlist watchlist)
    {
        // CHECK TO SEE IF THE MOVIE EXISTS
        var watchlistMovie = GetMovieFromWatchlist(watchlist);

        if (watchlistMovie == null)
        {
            return false;
        }

        //UPDATE STATUS
        _watchlistRepository.Update(watchlistMovie);
        return true;

    }
    
    // REMOVE MOVIE FROM WATCHLIST
    public bool RemoveFromWatchlist(Watchlist watchlist)
    {
        //CHECK IF MOVIE EXISTS
        var movieWatchlist = GetMovieFromWatchlist(watchlist);

        if (movieWatchlist == null)
        {
            return false;
        }
        
        // REMOVE MOVIE FROM WATCHLIST
        _watchlistRepository.Delete(movieWatchlist);
        return true;

    }
}