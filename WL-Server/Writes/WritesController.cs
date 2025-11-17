using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WL_Server.Writes;

[ApiController]
[Route("api/[controller]")]
public class WritesController : ControllerBase
{
    
    // IMPLEMENT WRITES SERVICE
    private readonly IWritesService _writesService;

    public WritesController(IWritesService writesService)
    {
        _writesService = writesService;
    }
    
    // FETCH WRITES FOR USER
    [HttpGet("{userId}")]
    public IActionResult GetUserWrites([FromQuery] int userId)
    {
        //STORE USERID FROM QUERY
        var input = new Writes();
        input.UserId = userId;

        try
        {
            //
            var writes = _writesService.FetchWritesByUserId(input);
            return Ok(writes);
        }
        catch
        {
            return Ok("Error");
        }
    }
    
    // FETCH WRITES FOR MOVIE
    [HttpGet("{movieId}")]
    public IActionResult GetMovieWrites([FromQuery] int movieId)
    {
        //STORE USERID FROM QUERY
        var input = new Writes();
        input.MovieId = movieId;

        try
        {
            //
            var writes = _writesService.FetchWritesByMovieId(input);
            return Ok(writes);
        }
        catch
        {
            return Ok("Error");
        }
    }
    
    // FETCH ALL WRITES
    [HttpGet("")]
    public IActionResult GetMovieWrites()
    {
        try
        {
            //
            var writes = _writesService.FetchAllWrites();
            return Ok(writes);
        }
        catch
        {
            return Ok("Error");
        }
    }

    [HttpPost("{userId}/{movieId}")]
    public IActionResult CreateWrites([FromQuery] int movieId, [FromQuery] int userId, [FromBody] float rating, [FromBody] string comment, [FromBody] DateTime datePosted, [FromBody] int upvoteCount)
    {
        try
        {
            //
            var input = new Writes();
            input.UserId = userId;
            input.MovieId = movieId;
            input.Rating = rating;
            input.Comment = comment;
            input.DatePosted = datePosted;
            input.UpvoteCount = upvoteCount;

            _writesService.CreateWrites(input);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }

    [HttpPut("{userID}/{movieID}")]
    public IActionResult UpdateUpvoteCount([FromRoute] int userId, [FromRoute] int movieId, [FromBody] int upvoteCount)
    {
        var input = new Writes();
        input.UserId = userId;
        input.MovieId = movieId;
        input.UpvoteCount = upvoteCount;

        try
        {
            _writesService.Upvote(input);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }

    [HttpDelete("{userId}/{movieId}")]
    public IActionResult DeleteWrites([FromRoute] int userId, [FromRoute] int movieId)
    {
        var input = new Writes();
        input.UserId = userId;
        input.MovieId = movieId;

        try
        {
            _writesService.DeleteWrites(input);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }
    

}