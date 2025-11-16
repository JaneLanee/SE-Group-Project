using Microsoft.AspNetCore.Mvc;

namespace WL_Server.Watchlist;

[ApiController]
[Route("api/[controller]")] //  https://localhost:5210/api/watchlist
public class WatchlistController : ControllerBase
{
    // GRAB WATCHLIST SERVICE FOR LOGIC CONTROLLERS
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    // FETCH THE USERS WATCHLIST
    [HttpGet("{userId}")] // https://localhost:5210/api/watchlist/{userID}
    public IActionResult GetWatchlist([FromQuery]int userId)
    {

        // STORE USERID FROM QUERY
        var input = new Watchlist();
        input.UserId = userId;

        try
        {
            // CALL TO REPOSITORY TO GRAB USER WATCHLIST
            var watchlist = _watchlistService.GetUserWatchlist(input);
            return Ok(watchlist);
        }
        catch // IN CASE OF ERROR
        {
            return Ok("Error");
        }
    }
    
    // GET REQUEST TO FETCH MOVIE FROM USERS WATCHLIST
    [HttpGet("{userId}/{movieId}")] // https://localhost:5210/api/watchlist/{userID}/{movieID}
    public IActionResult GetMovieFromWatchlist([FromQuery] int userId, [FromRoute] int movieId)
    {
        // STORE USERID AND MOVIEID FROM QUERY
        var input = new Watchlist();
        input.UserId = userId;
        input.MovieId = movieId;
        
        try
        {
            // FETCH MOVIE FROM USERS WATCHLIST
            var movie = _watchlistService.GetMovieFromWatchlist(input);
            return Ok(movie);
        }
        catch
        {
            return Ok("Error");
        }
    }
    
    // POST REQUEST TO ADD A MOVIE TO USERS WATCHLIST
    [HttpPost("{userId}/{movieId}/add")] // https://localhost:5210/api/watchlist/{userID}/{movieID}/add
    public IActionResult AddToWatchlist([FromQuery] int userId, [FromRoute] int movieId, [FromBody] float rating)
    {
        var input = new Watchlist();
        input.UserId = userId;
        input.MovieId = movieId;
        input.DateAdded = DateTime.UtcNow;
        input.PersonalRating = rating;

        try
        {
            //ADD MOVIE TO USER WATCHLIST
            _watchlistService.AddToWatchlist(input);
            return Ok("Success");

        }
        catch
        {
            return Ok("Error");
        }
    }

    // PUT REQUEST TO UPDATE THE STATUS OF A MOVIE IN USER WATCHLIST
    [HttpPut("{userId}/{movieID}/update")] // https://localhost:5210/api/watchlist/{userID}/{movieID}/update
    public IActionResult UpdateStatus([FromQuery] int userId, [FromRoute] int movieId, [FromBody] string status)
    {
        // STORE INFO
        var input = new Watchlist();
        input.UserId = userId;
        input.MovieId = movieId;
        input.Status = Enum.Parse<Watchlist.StatusType>(status);

        try
        {
            _watchlistService.UpdateStatus(input);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }

    // DELETE REQUEST TO REMOVE MOVIE FROM USER WATCHLIST
    [HttpDelete("{userId}/{movieId}/delete")] // https://localhost:5210/api/watchlist/{userID}/{movieID}/delete
    public IActionResult RemoveFromWatchlist([FromQuery] int userId, [FromRoute] int movieId)
    {
        var input = new Watchlist();
        input.UserId = userId;
        input.MovieId = movieId;

        try
        {
            _watchlistService.RemoveFromWatchlist(input);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }
}