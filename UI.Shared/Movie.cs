namespace UI.Shared;

public record Movie(long Id, long TmdbId, string Title, string Description, long Year, string PosterUri, string[] Genres, double Budget, string[] OriginCountries);