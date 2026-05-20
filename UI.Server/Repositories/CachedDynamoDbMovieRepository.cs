using LaPelicula.UI.Server.Common;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public class CachedDynamoDbMovieRepository(
    INonCached<IMovieRepository> innerRepository,
    IMemoryCache cache,
    IOptions<RepositoryCacheConfig> cacheOptions) : IMovieRepository
{
    private readonly IMemoryCache _cache = cache;
    private readonly TimeSpan _cacheDuration = cacheOptions.Value.CacheDuration;

    public async Task<Movie> GetMovieByIdAsync(long movieId)
    {
        var key = $"movie:{movieId}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.SetAbsoluteExpiration(_cacheDuration);
            return await innerRepository.Instance.GetMovieByIdAsync(movieId);
        }) ?? throw new KeyNotFoundException($"Movie with key {movieId} not found");
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        return (await _cache.GetOrCreateAsync("movies:all", async entry =>
        {
            entry.SetAbsoluteExpiration(_cacheDuration);
            return await innerRepository.Instance.GetMoviesAsync();
        }))!;
    }
}
