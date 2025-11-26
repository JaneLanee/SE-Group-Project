using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WL_Server.Watchlist;

public class WatchlistDTO
{
    public int? UserId { get; set; }
    public int? MovieId { get; set; }
    public string? MovieTitle { get; set; }
}

//[Authorize(Policy = "UserOnly")]
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
    [HttpGet("userwatchlist")] // https://localhost:5210/api/watchlist/{userID}
    public IActionResult GetWatchlist()
    {

        // STORE USERID FROM QUERY
        var input = new Watchlist();
        input.UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        Console.WriteLine(input.UserId);
        
        try
        {
            // CALL TO REPOSITORY TO GRAB USER WATCHLIST
            var watchlist = _watchlistService.GetUserWatchlist(input);
            return Ok(watchlist);
        }
        catch(Exception e) // IN CASE OF ERROR
        {
            return Ok(e.Message);
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
    [HttpPost("add")] // https://localhost:5210/api/watchlist/{userID}/{movieID}/add
    public IActionResult AddToWatchlist([FromBody] WatchlistDTO input)
    {

        var watchlist = new Watchlist();
        input.UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        watchlist.UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        Console.WriteLine(watchlist.UserId);
        watchlist.MovieId = input.MovieId;
        watchlist.DateAdded = DateTime.Now;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            try
            {
                //ADD MOVIE TO USER WATCHLIST
                _watchlistService.AddToWatchlist(watchlist, input.MovieTitle);
                return Ok("Success: Added movie to watchlist");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Ok("Error");
            }
        }

        return BadRequest("Login required");

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