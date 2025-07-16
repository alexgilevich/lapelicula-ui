namespace UI.Shared;

public interface IRecommendationsHttpService
{
    Task<List<Recommendation>> GetRecommendations();
}