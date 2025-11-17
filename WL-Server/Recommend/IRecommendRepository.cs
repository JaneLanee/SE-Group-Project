namespace WL_Server.Recommend;

public interface IRecommendRepository
{
    public Recommend GetMovieById(Recommend movie);

    public Recommend[] GetRecommendList(int year);

    public bool AddMovie(Recommend movie);

}