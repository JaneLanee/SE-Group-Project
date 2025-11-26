using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WL_Server.User;

public class UserDTO
{
    public string? Username { get; set; }
    public string? UserId { get; set; }
}

//THIS FILE WILL HANDLE REQUESTS AND HANDLE RESPONSES WITH THE USER INTERFACE (FRONTEND)
//STRICTLY FOR API RELATED ACTIVITY
[ApiController]
[Route("api/[controller]")] // https://localhost:5210/api/user/
public class UserController : ControllerBase
{
    
    //GRAB USER SERVICE FOR BUSINESS LOGIC IN CONTROLLERS
    private readonly IUserService _userService;
    private HttpContext _session;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    //USER SIGNUP
    [HttpPost("signup")] // https://localhost:5210/api/user/signup
    public async Task<IActionResult> SignUp([FromBody] User input)
    {
        var userResults =  _userService.SignUp(input);
        Console.WriteLine(userResults.Username);

        if (userResults == null)
        {
            return Ok("Error");
        }
        
        var claims = new List<Claim>
        {
            //new Claim("LoggedId", userResults.ID.ToString()),
            new Claim("LoggedUser", userResults.Username),
            new Claim(ClaimTypes.NameIdentifier, userResults.ID.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            "CustomCookieOk");
        
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)

        };

        Console.WriteLine("its over");
        try
        {
            await HttpContext.SignInAsync("CustomCookieOk",
                claimsPrincipal);
            
            Console.WriteLine(HttpContext.User.FindFirstValue("LoggedUser"));
            return Ok("Logged in: " +  HttpContext.User.FindFirstValue("LoggedUser"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Ok("Error");
        }
        
        
    }
    
    //USER LOGIN
    [HttpPost("login")] // https://localhost:5210/api/user/login
    public async Task<IActionResult> Login([FromBody] User input)
    {
        Console.WriteLine(input.Username + " From input");

            var userResults = _userService.Login((input));
            Console.WriteLine(userResults.Username);

            if (userResults == null)
            {
                return Ok("User not found");
            }

            string username = userResults.Username;

            var claims = new List<Claim>
            {
                //new Claim("LoggedId", userResults.ID.ToString()),
                new Claim("LoggedUser", username),
                new Claim(ClaimTypes.NameIdentifier, userResults.ID.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                "CustomCookieOk");

            var authProperties = new AuthenticationProperties();

            Console.WriteLine("its over");
            try
            {
                await HttpContext.SignInAsync("CustomCookieOk", new ClaimsPrincipal(claimsIdentity));
                
                //Console.WriteLine("Logged in {User} !", userResults.Username);

                Console.WriteLine(HttpContext.User.FindFirstValue("LoggedUser"));
                return Ok("Logged in: " +  HttpContext.User.FindFirstValue("LoggedUser"));
            }
            catch (Exception e)
            {
                return Ok($"Error: {e}");
            }
            //var loggedUsername = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
    
    //GET LOGGED USER
    [HttpGet("loggedUser")]
    public IActionResult LoggedUser()
    {
        try
        {
            var loggedUser = new UserDTO();
            loggedUser.Username = HttpContext.User.FindFirstValue("LoggedUser");
            loggedUser.UserId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(loggedUser);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
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