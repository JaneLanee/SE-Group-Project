namespace WL_Server.Other;

public interface IRepository<T>
{
    void Get(T entity);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}