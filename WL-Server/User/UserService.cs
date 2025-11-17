using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WL_Server.User;


//THIS FILE IS TO HANDLE THE APP AND BUSINESS LOGIC FOR THE USER MODEL
//THE USER CONTROLLER FILE
public class UserService : IUserService
{
    //GET USER REPOSITORY
    private IUserRepository _userRepository;
    private IHttpContextAccessor _httpContextAccessor;

    public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool SignUp(User input)
    {
        Console.WriteLine(input.username + " lets see");

        //CHECK IF EMAIL OR USERNAME ARE ALREADY TAKEN
        if (_userRepository.GetByUsername(input).username == null)
        {
            Console.WriteLine("First check passed ..");
        }
        else //USERNAME EXISTS
        {
            Console.WriteLine("username exists ...");
            return false;
        }
        
        Console.WriteLine("First check ..");
        if (_userRepository.GetByEmail(input).email == null)
        {
            Console.WriteLine("Second check passsed ..");
            try
            {
                //CREATE USER IF PASS
                Console.WriteLine(input.username + " about to create ...");
                _userRepository.Create(input);
                //STORE SESSION / AUTHENTICATE
                SetSession(input.ID);
            }
            catch //IN CASE OF ERROR
            {
                return false;
            }
        }
        else //EMAIL EXISTS
        {
            Console.WriteLine("email exists ...");
            return false;
        }

        return true;
    }
    
    //USER LOGIN
    public bool Login(User user)
    {
        //CHECK IF EMAIL OR USERNAME EXIST
        var checkUserByEmail = _userRepository.GetByEmail(user);
        var checkUserByUsername = _userRepository.GetByEmail(user);
        if (checkUserByEmail.email != null)
        {
            if (user.PwHash == checkUserByEmail.PwHash)
            {
                //LOGIN USER
                //STORE SESSION / AUTHENTICATE
                SetSession(checkUserByUsername.ID);
                return true;
            } else //WRONG PASSWORD
            {
                return false;
            }
        }

        if (checkUserByUsername.username != null)
        {
            if (user.PwHash == checkUserByUsername.PwHash)
            {
                //LOGIN USER
                //STORE SESSION / AUTHENTICATE
                SetSession(checkUserByUsername.ID);
                return true;
            }
        }
        else //USER LOGIN FAILED
        {
            Console.WriteLine("FALSE");
            return false;
        }

        return false;
    }
    
    //HASH PASSWORD
    public string HashPassword(string password)
    {

        byte[] salt = new byte[16];
        salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashedPW = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested:256 / 8));
        
        return hashedPW;
    }
    
    //FETCH USER INFO BY USERNAME
    public User FetchUserByUsername(string username)
    {
        var findUser = new User();
        findUser.username = username;

        //CHECK IF USER EXISTS
        var user = _userRepository.GetByUsername(findUser);
        
        if (user != null)
        {
            return user;
        }

        return null;
    }

    //STORE INFO INTO SESSION FOR AUTHENTICATION/AUTHORIZATION
    public void SetSession(int userId)
    {

        string sessionId = "";

        //STORE INFO FOR SESSIONS
        //USED FOR USER AUTH
        _httpContextAccessor.HttpContext.Session.SetInt32("UserId", userId);

        Guid uuid = new Guid();
        string sessionKey = uuid.ToString();
        
        //STORE SESSION IN DATABASE
        _httpContextAccessor.HttpContext.Session.SetString("SessionKey", sessionKey);

        _userRepository.StoreSession(userId, sessionId, DateTime.Now);

    }

    //CHECK SESSION DATA TO SEE IF USER IS LOGGED IN
    public bool GetSession()
    {
        //CHECK IS USER IS LOGGED IN SESSION
        var loggedUser = _httpContextAccessor.HttpContext.Session.GetString("SessionKey");

        if (loggedUser != null)
        {
            Console.WriteLine("User login required");
            return false;
        }

        return true;

    }
}