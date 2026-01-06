using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface IMovieRepository
{
    Task<Movie> GetMovieByIdAsync(long movieId);
    Task<IEnumerable<Movie>> GetMoviesAsync();
}

/// <summary>
/// DynamoDB-backed movie repository. On first use, scans the entire table and caches movies in memory.
/// Environment:
///   MOVIES_TABLE – required, name of DynamoDB table
///   AWS_REGION   – optional, region for the client (falls back to SDK defaults)
/// </summary>
public class DynamoDbMovieRepository : IMovieRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public DynamoDbMovieRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb;
    }

    public async Task<Movie> GetMovieByIdAsync(long movieId)
    {
        var response = await _dynamoDb.GetItemAsync("movies", new Dictionary<string, AttributeValue>() { { "movie_id", new AttributeValue { N = movieId.ToString() } } });
        return  (response.IsItemSet 
            ? MapItemToMovie(response.Item) 
            : throw new KeyNotFoundException($"Movie with key {movieId} not found"))!; // IsItemSet is guaranteed to be true when Item is set
    }

    
    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        var request = new ScanRequest
        {
            TableName = "movies"
        };
        
        var res = new List<Movie>();
        do
        {
            var response = await _dynamoDb.ScanAsync(request);
            foreach (var item in response.Items)
            {
                var movie = MapItemToMovie(item);
                if (movie is null) 
                    continue;
                res.Add(movie);
            }

            request.ExclusiveStartKey = response.LastEvaluatedKey;
        } while (request.ExclusiveStartKey is { Count: > 0 });

        return res;
    }


    private static Movie? MapItemToMovie(Dictionary<string, AttributeValue> item)
    {
        if (!item.TryGetValue("movie_id", out var idAttr) || idAttr.N == null)
        {
            return null;
        }
        
        long id = SafeLong(item, "movie_id");
        long tmdbId = SafeLong(item, "tmdb_id");
        string title = SafeString(item, "title");
        string description = SafeString(item, "description");
        long year = SafeLong(item, "year");
        double ratingAverage = SafeDouble(item, "rating_avg");
        string posterUri = SafeString(item, "poster_uri");
        double budget = SafeDouble(item, "budget");
        string[] originCountries = SafeStringList(item, "origin_countries");
        string[] genres = SafeStringList(item, "genres");

        return new Movie(
            id,
            tmdbId,
            title,
            description,
            year,
            posterUri,
            genres,
            budget,
            originCountries,
            ratingAverage
        );
    }

    private static string SafeString(Dictionary<string, AttributeValue> item, string key)
        => item.TryGetValue(key, out var a) && a.S != null ? a.S : string.Empty;

    private static long SafeLong(Dictionary<string, AttributeValue> item, string key)
        => item.TryGetValue(key, out var a) && a.N != null && long.TryParse(a.N, out var v) ? v : 0L;

    private static double SafeDouble(Dictionary<string, AttributeValue> item, string key)
        => item.TryGetValue(key, out var a) && a.N != null && double.TryParse(a.N, out var v) ? v : 0d;

    private static string[] SafeStringList(Dictionary<string, AttributeValue> item, string key)
    {
        if (!item.TryGetValue(key, out var a) || a.L == null) return Array.Empty<string>();
        return a.L.Select(av => av.S).Where(s => s != null).ToArray()!;
    }
}