namespace WL_Server.User;


//THIS FILE IS TO HANDLE THE APP AND BUSINESS LOGIC FOR THE USER MODEL
//THE USER CONTROLLER FILE
public class UserService : IUserService
{
    //GET USER REPOSITORY
    private IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
                return true;
                
                //STORE SESSION / AUTHENTICATE
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
                return true;
                
                //STORE SESSION / AUTHENTICATE
            }
        }
        else //USER LOGIN FAILED
        {
            Console.WriteLine("FALSE");
            return false;
        }

        return false;
    }

    //test ignore
    public User TestGrab(User input)
    {
        input.username = "test";
        
        
        var result = _userRepository.GetByUsername(input);
        return result;
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
}