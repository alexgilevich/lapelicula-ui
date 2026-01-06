namespace UI.Shared;


public record Movie
{
    public Movie() { }
    
    public Movie(long Id, long TmdbId, string Title, string Description, long Year, string PosterUri, string[] Genres, double Budget, string[] OriginCountries, double RatingAverage)
    {
        this.Id = Id;
        this.TmdbId = TmdbId;
        this.Title = Title;
        this.Description = Description;
        this.Year = Year;
        this.PosterUri = PosterUri;
        this.Budget = Budget;
        this.OriginCountries = OriginCountries;
        this.RatingAverage = RatingAverage;
        RawGenres = Genres;
        GenreVector = GetGenreVector(Genres);
    }

    #region Non-genre movie properties
    
    public long Id { get; init; }
    public long TmdbId { get; init; } 
    public string Title { get; init; } 
    public string Description { get; init; }
    public long Year { get; init; }
    public string PosterUri { get; init; }
    public double Budget { get; init; }
    public string[] OriginCountries { get; init; }
    public double RatingAverage { get; init; }
    
    #endregion

    #region Genre-related movie properties

    public string[] RawGenres { get; init; }
    public byte[] GenreVector { get; init; }
    public bool Action => GenreVector[Genre.Action.Position] > 0;
    public bool Adventure => GenreVector[Genre.Adventure.Position] > 0;
    public bool Animation => GenreVector[Genre.Animation.Position] > 0;
    public bool Comedy => GenreVector[Genre.Comedy.Position] > 0;
    public bool Crime => GenreVector[Genre.Crime.Position] > 0;
    public bool Documentary => GenreVector[Genre.Documentary.Position] > 0;
    public bool Drama => GenreVector[Genre.Drama.Position] > 0;
    public bool Fantasy => GenreVector[Genre.Fantasy.Position] > 0;
    public bool FilmNoir => GenreVector[Genre.FilmNoir.Position] > 0;
    public bool Horror => GenreVector[Genre.Horror.Position] > 0;
    public bool Kids => GenreVector[Genre.Kids.Position] > 0;
    public bool Musical => GenreVector[Genre.Musical.Position] > 0;
    public bool Mystery => GenreVector[Genre.Mystery.Position] > 0;
    public bool Romance => GenreVector[Genre.Romance.Position] > 0;
    public bool SciFi => GenreVector[Genre.SciFi.Position] > 0;
    public bool Thriller => GenreVector[Genre.Thriller.Position] > 0;
    public bool War => GenreVector[Genre.War.Position] > 0;
    public bool Western => GenreVector[Genre.Western.Position] > 0;
    
    #endregion
    
    
    private static byte[] GetGenreVector(string[] genres)
    {
        var genreVector = new byte[Genre.Count];
        foreach (var rawGenre in genres)
        {
            switch (rawGenre)
            {
                case Genre.Names.Action:
                    genreVector[Genre.Action.Position] = 1;
                    break;
                case Genre.Names.Adventure:
                    genreVector[Genre.Adventure.Position] = 1;
                    break;
                case Genre.Names.Animation:
                    genreVector[Genre.Animation.Position] = 1;
                    break;
                case Genre.Names.Comedy:
                    genreVector[Genre.Comedy.Position] = 1;
                    break;
                case Genre.Names.Crime:
                    genreVector[Genre.Crime.Position] = 1;
                    break;
                case Genre.Names.Documentary:
                    genreVector[Genre.Documentary.Position] = 1;
                    break;
                case Genre.Names.Drama:
                    genreVector[Genre.Drama.Position] = 1;
                    break;
                case Genre.Names.Fantasy:
                    genreVector[Genre.Fantasy.Position] = 1;
                    break;
                case Genre.Names.FilmNoir:
                    genreVector[Genre.FilmNoir.Position] = 1;
                    break;
                case Genre.Names.Horror:
                    genreVector[Genre.Horror.Position] = 1;
                    break;
                case Genre.Names.Kids:
                    genreVector[Genre.Kids.Position] = 1;
                    break;
                case Genre.Names.Musical:
                    genreVector[Genre.Musical.Position] = 1;
                    break;
                case Genre.Names.Mystery:
                    genreVector[Genre.Mystery.Position] = 1;
                    break;
                case Genre.Names.Romance:
                    genreVector[Genre.Romance.Position] = 1;
                    break;
                case Genre.Names.SciFi:
                    genreVector[Genre.SciFi.Position] = 1;
                    break;
                case Genre.Names.Thriller:
                    genreVector[Genre.Thriller.Position] = 1;
                    break;
                case Genre.Names.War:
                    genreVector[Genre.War.Position] = 1;
                    break;
                case Genre.Names.Western:
                    genreVector[Genre.Western.Position] = 1;
                    break;
            }
        }

        return genreVector;
    }
}
