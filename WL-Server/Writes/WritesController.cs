using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WL_Server.Writes;

public class WritesDTO
{
    public int? MovieId { get; set; }
    
    public string? MovieTitle { get; set; }
    
    public float? Rating { get; set; }
    
    public string? Comment { get; set; }
    
}

//[Authorize(Policy = "UserOnly")]
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
    [HttpGet("userWrites")]
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
    public IActionResult GetMovieWrites([FromRoute] int movieId)
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
        catch(Exception e)
        {
            Console.WriteLine(e);
            return Ok("Error");
        }
    }
    
    // FETCH ALL WRITES
    [HttpGet("")]
    public IActionResult GetAllWrites()
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

    //CREATE WRITES
    [HttpPost("add")]
    public IActionResult CreateWrites([FromBody] WritesDTO writes)
    {
        try
        {
            //
            var input = new Writes();
            input.UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            input.MovieId = writes.MovieId;
            input.Rating = writes.Rating;
            input.Comment = writes.Comment;
            input.DatePosted = DateTime.Now;
            input.UpvoteCount = 0;

            _writesService.CreateWrites(input, writes.MovieTitle);
            return Ok("Success");
        }
        catch
        {
            return Ok("Error");
        }
    }

    [HttpPut("update")]
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

    [HttpDelete("delete")]
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