using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WL_Server.User;


//THIS FILE IS TO HANDLE THE APP AND BUSINESS LOGIC FOR THE USER MODEL
//THE USER CONTROLLER FILE
public class UserService : IUserService
{
    //GET USER REPOSITORY
    private IUserRepository _userRepository;
    private HttpContext _session;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public User SignUp(User input)
    {

        //CHECK IF EMAIL OR USERNAME ARE ALREADY TAKEN
        if (_userRepository.GetByUsername(input).Username == null)
        {
        }
        else //USERNAME EXISTS
        {
            Console.WriteLine("username exists ...");
        }
        
        Console.WriteLine("First check ..");
        if (_userRepository.GetByEmail(input).Email == null)
        {
            Console.WriteLine("Second check passsed ..");
            try
            {
                //CREATE USER IF PASS
                input.Password = HashPassword2(input.Password);
                _userRepository.Create(input);
            }
            catch (Exception e) //IN CASE OF ERROR
            {
                Console.WriteLine(e.Message);
            }
        }
        else  //EMAIL EXISTS
        {
            Console.WriteLine("email exists ...");
        }

        return input;
    }
    
    //USER LOGIN
    public User Login(User user)
    {
        Console.WriteLine(user.Username + " From user service");
        //CHECK IF EMAIL OR USERNAME EXIST
        var checkUserByEmail = _userRepository.GetByEmail(user);
        var checkUserByUsername = _userRepository.GetByUsername(user);
        Console.WriteLine(checkUserByUsername.Email);
        if (checkUserByUsername.Username != null)
        {
            Console.WriteLine("You made it !! lets check password ..");

            try
            {
                var inputPassword = VerifyPassword(user.Password, checkUserByUsername.Password);
                Console.WriteLine(inputPassword);

                if (inputPassword)
                {
                    // RETURN LOGGED IN USER INFO
                    return checkUserByUsername;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        Console.WriteLine("LOGIN FAILED (wrong username/email or password)");
        return checkUserByUsername;
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

    public string HashPassword2(string password)
    {
        //INITIALIZE SALT SIZE< HASH SIZE, AND ITERATION COUNT
        int saltSize = 16;
        int hashSize = 32;
        int iterationCount = 100000;
        
        //INITIALIZE HASH ALGO (WE'RE USING SHA256 FOR THIS METHOD)
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        
        //GENERATE RANDOM NUMBERS FOR HASH
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        
        //COMBINE INTO ONE HASH
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterationCount, hashAlgorithm, hashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";


    }
    
    //VERIFY PASSWORD
    public bool VerifyPassword(string password, string hashedPassword)
    {
        //INITIALIZE SALT SIZE< HASH SIZE, AND ITERATION COUNT
        int saltSize = 16;
        int hashSize = 32;
        int iterationCount = 100000;
        
        //INITIALIZE HASH ALGO (WE'RE USING SHA256 FOR THIS METHOD)
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA256;
        
        //SPLIT THE HASHED PASSWORD INTO TWO PARTS
        string[] parts = hashedPassword.Split('-');
        byte[] hash = Convert.FromHexString(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);
        
        //TURN INP
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterationCount, hashAlgorithm, hashSize);
        
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        
    }
    
    //FETCH USER INFO BY USERNAME
    public User FetchUserByUsername(string username)
    {
        var findUser = new User();
        findUser.Username = username;

        //CHECK IF USER EXISTS
        var user = _userRepository.GetByUsername(findUser);
        
        if (user != null)
        {
            return user;
        }

        return null;
    }
    
    //AUTHENTICATE USER WITH COOKIES
    public void AuthenticateUser(User user)
    {
        try
        {
            Console.WriteLine("init claims ..");
            Console.WriteLine(user.Username);

            string username = user.Username;
            //INIT A NEW CLAIM
            var claims = new List<Claim>
            {
                new Claim("LoggedId", user.ID.ToString()),
                new Claim("LoggedUser", username),
                //new Claim("Username", user.Username),
            };

            Console.WriteLine("now do this for " + user.Username);
            //
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            
            var authProperties = new AuthenticationProperties();

            Console.WriteLine("now do THIS ...");
            //AUTHENTICATE USER AND STORE
            _session.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
            
            Console.WriteLine("User authenticated !!!!");
        }
        catch (Exception e)
        {
            //RETURN TO LOGIN//SIGNUP
            Console.WriteLine("AUTH ERROR MESSAGE" + e.Message);
        }
    }

    //STORE INFO INTO SESSION FOR AUTHENTICATION/AUTHORIZATION
    public void SetSession(int userId)
    {

        string sessionId = "";

        //STORE INFO FOR SESSIONS
        //USED FOR USER AUTH
        //_httpContextAccessor.HttpContext.Session.SetInt32("UserId", userId);
        _session.Session.SetInt32("UserId", userId);

        Guid uuid = new Guid();
        string sessionKey = uuid.ToString();
        
        //STORE SESSION IN DATABASE
        //_httpContextAccessor.HttpContext.Session.SetString("SessionKey", sessionKey);
        _session.Session.SetString("SessionKey", sessionKey);

        _userRepository.StoreSession(userId, sessionKey, DateTime.Now);

    }

    //CHECK SESSION DATA TO SEE IF USER IS LOGGED IN
    public bool GetSession()
    {
        //CHECK IS USER IS LOGGED IN SESSION
        //var loggedUser = _session.HttpContext.Session.GetString("SessionKey");
        var loggedUser = _session.Session.GetString("SessionKey");

        if (loggedUser != null)
        {
            Console.WriteLine("User login required");
            return false;
        }

        return true;

    }
}