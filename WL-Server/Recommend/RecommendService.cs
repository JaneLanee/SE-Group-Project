namespace WL_Server.Recommend;

public class RecommendService : IRecommendService
{
    
    private readonly IRecommendRepository _recommendRepository;

    public RecommendService(IRecommendRepository recommendRepository)
    {
        _recommendRepository = recommendRepository;
    }
    
    public Recommend[] FetchRecommendedList(int year)
    {
        return _recommendRepository.GetRecommendList(year);
    }
}