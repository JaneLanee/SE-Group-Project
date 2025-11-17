namespace WL_Server.Recommend;

public interface IRecommendService
{
    public Recommend[] FetchRecommendedList(int year);
}