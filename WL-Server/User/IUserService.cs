namespace WL_Server.User;

public interface IUserService
{
    public bool SignUp(User user);

    public bool Login(User user);

    public User TestGrab(User user);
}