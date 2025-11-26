namespace WL_Server.Watchlist;

//ABSTRACT METHODS FOR WATCHLIST DATABASE LOGIC
public interface IWatchlistRepository
{
    public Watchlist[] GetWatchlist(Watchlist watchlist);

    public Watchlist GetWatchlistById(Watchlist watchlist);

    public int GetMovieById(int movieId);
    
    public bool AddMovie(int movieId, string movieTitle);
    
    public void Create(Watchlist watchlist);

    public void Update(Watchlist watchlist);

    public void Delete(Watchlist watchlist);
}