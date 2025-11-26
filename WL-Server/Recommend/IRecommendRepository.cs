namespace WL_Server.Recommend;

public interface IRecommendRepository
{
    public Recommend GetMovieById(int movie);

    public Recommend[] GetRecommendList(int year);

    public bool AddMovie(int movieId, string movieTitle);

}