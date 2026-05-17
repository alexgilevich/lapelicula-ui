namespace UI.Shared;

public record Genre(string Name, byte Position)
{
    public const int Count = 18;
    public static class Names
    {
        public const string Action = "Action";
        public const string Adventure = "Adventure";
        public const string Animation = "Animation";
        public const string Comedy = "Comedy";
        public const string Crime = "Crime";
        public const string Documentary = "Documentary";
        public const string Drama = "Drama";
        public const string Fantasy = "Fantasy";
        public const string FilmNoir = "Film-Noir";
        public const string Horror = "Horror";
        public const string Kids = "Kids";
        public const string Musical = "Musical";
        public const string Mystery = "Mystery";
        public const string Romance = "Romance";
        public const string SciFi = "Sci-Fi";
        public const string Thriller = "Thriller";
        public const string War = "War";
        public const string Western = "Western";
    }

    public static readonly Genre Action = new (Names.Action, 0);
    public static readonly Genre Adventure = new (Names.Adventure, 1);
    public static readonly Genre Animation = new (Names.Animation, 2);
    public static readonly Genre Comedy = new (Names.Comedy, 3);
    public static readonly Genre Crime = new (Names.Crime, 4);
    public static readonly Genre Documentary = new (Names.Documentary, 5);
    public static readonly Genre Drama = new (Names.Drama, 6);
    public static readonly Genre Fantasy = new (Names.Fantasy, 7);
    public static readonly Genre FilmNoir = new (Names.FilmNoir, 8);
    public static readonly Genre Horror = new (Names.Horror, 9);
    public static readonly Genre Kids = new (Names.Kids, 10);
    public static readonly Genre Musical = new (Names.Musical, 11);
    public static readonly Genre Mystery = new (Names.Mystery, 12);
    public static readonly Genre Romance = new (Names.Romance, 13);
    public static readonly Genre SciFi = new (Names.SciFi, 14);
    public static readonly Genre Thriller = new (Names.Thriller, 15);
    public static readonly Genre War = new (Names.War, 16);
    public static readonly Genre Western = new (Names.Western, 17);
}