namespace WL_Server.User;

public interface IUserService
{
    public User SignUp(User user);

    public User Login(User user);

    public string HashPassword(string password);

    public User FetchUserByUsername(string username);

    public void SetSession(int userId);
    public bool GetSession();
}