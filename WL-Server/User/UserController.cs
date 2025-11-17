using Microsoft.AspNetCore.Mvc;

namespace WL_Server.User;

//THIS FILE WILL HANDLE REQUESTS AND HANDLE RESPONSES WITH THE USER INTERFACE (FRONTEND)
//STRICTLY FOR API RELATED ACTIVITY
[ApiController]
[Route("api/[controller]")] // https://localhost:5210/api/user/
public class UserController : ControllerBase
{
    
    //GRAB USER SERVICE FOR BUSINESS LOGIC IN CONTROLLERS
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    //USER SIGNUP
    [HttpPost("signup")] // https://localhost:5210/api/user/signup
    public IActionResult SignUp([FromBody] User input)
    {
        try
        {
            Console.WriteLine(input.username + " welcome ...");
            _userService.SignUp(input);
            return Ok("ok");
        }
        catch
        {
            return Ok("Error !!!");
        }
    }
    
    //USER LOGIN
    [HttpPost("login")] // https://localhost:5210/api/user/login
    public IActionResult Login([FromBody] User input)
    {
        try
        {
            _userService.Login((input));
            return Ok("");
        }
        catch
        {
            return Ok("Error");
        }
    }
    
    //GET USER INFO BY USERNAME
    [HttpGet("{username}")] // https://localhost:5210/api/user/{username}
    public IActionResult FetchUser(string username)
    {
        
        try
        {
            var findUser = _userService.FetchUserByUsername(username);

            if (findUser != null)
            {
                return Ok(findUser);
            }
            
            return Ok("User not found");
        }
        catch
        {
            return Ok("Error");
        }
    }
    
}