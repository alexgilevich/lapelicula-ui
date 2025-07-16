using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using UI.Shared;

namespace LaPelicula.UI.Client.Services;

/// <summary>
/// Stub class returning random movies the top 200 static list for SEO purposes mostly
/// </summary>
public class PrerenderedRecommendationsHttpService : IRecommendationsHttpService
{
    private record TopMovie(
        long TmdbId,
        string Title,
        string Description,
        long Year,
        string PosterUri,
        string Genres,
        double Budget,
        string OriginCountries,
        float RatingAvg)
    {
        public string[] OriginCountriesArr => OriginCountries.Split(',');
        public string[] GenresArr => OriginCountries.Split(',');
    }
    
    private TopMovie[] _topMovies;
    public PrerenderedRecommendationsHttpService()
    {
        LoadTop200();
    }

    private void LoadTop200()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => Regex.Replace(
                args.Header,
                "(?<=[\\W_]|^)\\w", match => match.Value.ToUpper()).Replace("_", string.Empty),
            
        };
        
        using (var reader = new StreamReader("./ml/data/top200_movies.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            var records = csv.GetRecords<TopMovie>();
            _topMovies = records.ToArray();
        }
    }

    public Task<List<Recommendation>> GetRecommendations()
    {
        var topMovies = (TopMovie[]) _topMovies.Clone();
        Random.Shared.Shuffle(topMovies);
        
        var recommendations = topMovies.Take(25).Select(m => new Recommendation(
            new Movie(m.TmdbId, m.TmdbId, m.Title, m.Description, m.Year, m.PosterUri, m.GenresArr, m.Budget,
                m.OriginCountriesArr),
            m.RatingAvg)
        ).ToList();
        
        return Task.FromResult(recommendations);
    }
}

