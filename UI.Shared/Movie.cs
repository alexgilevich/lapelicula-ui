namespace UI.Shared;


public record Movie
{
    private static byte[] DefaultGenreVector = new byte[Genre.Count];

    #region Non-genre movie properties

    public required long Id { get; init; }
    public required long TmdbId { get; init; }
    public required long ImdbId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Tagline { get; init; }
    public required long Year { get; init; }
    public required string PosterUri { get; init; }
    public required decimal Budget { get; init; }
    public required decimal Revenue { get; init; }
    public required string[] OriginCountries { get; init; }
    public required string[] ProductionCountries { get; init; }
    public required double RatingAverage { get; init; }
    public required bool Adult { get; init; }


    #endregion

    #region Genre-related movie properties

    public required string[] RawGenres 
    { 
        get; 
        init 
        {
            field = value;
            _genreVector = GetGenreVector(value);
        } 
    }
    private byte[] _genreVector = DefaultGenreVector;
    public byte[] GenreVector => _genreVector;
    public bool Action => _genreVector[Genre.Action.Position] > 0;
    public bool Adventure => _genreVector[Genre.Adventure.Position] > 0;
    public bool Animation => _genreVector[Genre.Animation.Position] > 0;
    public bool Comedy => _genreVector[Genre.Comedy.Position] > 0;
    public bool Crime => _genreVector[Genre.Crime.Position] > 0;
    public bool Documentary => _genreVector[Genre.Documentary.Position] > 0;
    public bool Drama => _genreVector[Genre.Drama.Position] > 0;
    public bool Fantasy => _genreVector[Genre.Fantasy.Position] > 0;
    public bool FilmNoir => _genreVector[Genre.FilmNoir.Position] > 0;
    public bool Horror => _genreVector[Genre.Horror.Position] > 0;
    public bool Kids => _genreVector[Genre.Kids.Position] > 0;
    public bool Musical => _genreVector[Genre.Musical.Position] > 0;
    public bool Mystery => _genreVector[Genre.Mystery.Position] > 0;
    public bool Romance => _genreVector[Genre.Romance.Position] > 0;
    public bool SciFi => _genreVector[Genre.SciFi.Position] > 0;
    public bool Thriller => _genreVector[Genre.Thriller.Position] > 0;
    public bool War => _genreVector[Genre.War.Position] > 0;
    public bool Western => _genreVector[Genre.Western.Position] > 0;
    
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
