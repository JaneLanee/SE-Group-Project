namespace WL_Server.Watchlist;

public interface IWatchlistService
{
    public Watchlist[] GetUserWatchlist(Watchlist watchlist);

    public Watchlist GetMovieFromWatchlist(Watchlist watchlist);

    public bool AddToWatchlist(Watchlist watchlist, string movieTitle);

    public bool UpdateStatus(Watchlist watchlist);

    public bool RemoveFromWatchlist(Watchlist watchlist);
}