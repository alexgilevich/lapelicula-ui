namespace LaPelicula.UI.Shared;


public class UserPreferences
{
    public double Action { get; init; }
    public double Adventure { get; init; }
    public double Animation { get; init; }
    public double Kids { get; init; }
    public double Comedy { get; init; }
    public double Crime { get; init; }
    public double Documentary { get; init; }
    public double Drama { get; init; }
    public double Fantasy { get; init; }
    public double Horror { get; init; }
    public double Mystery { get; init; }
    public double Romance { get; init; }
    public double Scifi { get; init; }
    public double Thriller { get; init; }

    public UserPreferences()
    {
    }
    
    public Dictionary<string, double> ToDictionary()
    {
        return new Dictionary<string, double>
        {
            { nameof(Action), Action },
            { nameof(Adventure), Adventure },
            { nameof(Animation), Animation },
            { nameof(Kids), Kids },
            { nameof(Comedy), Comedy },
            { nameof(Crime), Crime },
            { nameof(Documentary), Documentary },
            { nameof(Drama), Drama },
            { nameof(Fantasy), Fantasy },
            { nameof(Horror), Horror },
            { nameof(Mystery), Mystery },
            { nameof(Romance), Romance },
            { nameof(Scifi), Scifi },
            { nameof(Thriller), Thriller }
        };
    }
    
    public static UserPreferences FromDictionary(IDictionary<string, double> dictionary)
    {
        return new UserPreferences
        {
            Action = dictionary[nameof(Action)],
            Adventure = dictionary[nameof(Adventure)],
            Animation = dictionary[nameof(Animation)],
            Kids = dictionary[nameof(Kids)],
            Comedy = dictionary[nameof(Comedy)],
            Crime = dictionary[nameof(Crime)],
            Documentary = dictionary[nameof(Documentary)],
            Drama = dictionary[nameof(Drama)],
            Fantasy = dictionary[nameof(Fantasy)],
            Horror = dictionary[nameof(Horror)],
            Mystery = dictionary[nameof(Mystery)],
            Romance = dictionary[nameof(Romance)],
            Scifi = dictionary[nameof(Scifi)],
            Thriller = dictionary[nameof(Thriller)],
        };
    }
    
    
}