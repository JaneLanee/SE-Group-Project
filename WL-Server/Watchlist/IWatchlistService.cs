namespace WL_Server.Watchlist;

public interface IWatchlistService
{
    public Watchlist[] GetUserWatchlist(Watchlist watchlist);

    public Watchlist GetMovieFromWatchlist(Watchlist watchlist);

    public bool AddToWatchlist(Watchlist watchlist);

    public bool UpdateStatus(Watchlist watchlist);

    public bool RemoveFromWatchlist(Watchlist watchlist);
}