namespace UI.Shared;

public record Genre(string Name, byte Position)
{
    public const int Count = 18;
    public static class Names
    {
        public const string Action = "action";
        public const string Adventure = "adventure";
        public const string Animation = "animation";
        public const string Comedy = "comedy";
        public const string Crime = "crime";
        public const string Documentary = "documentary";
        public const string Drama = "drama";
        public const string Fantasy = "fantasy";
        public const string FilmNoir = "film_noir";
        public const string Horror = "horror";
        public const string Kids = "kids";
        public const string Musical = "musical";
        public const string Mystery = "mystery";
        public const string Romance = "romance";
        public const string SciFi = "sci_fi";
        public const string Thriller = "thriller";
        public const string War = "war";
        public const string Western = "western";
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