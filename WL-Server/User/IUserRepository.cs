namespace WL_Server.User;

public interface IUserRepository
{
    User GetById(User entity);
    
    User GetByUsername(User entity);
    
    User GetByEmail(User entity);
    
    void GetAll(User entity);
    
    void Create(User entity);

    bool StoreSession(int userId, string sessionId, DateTime createdAt);
}