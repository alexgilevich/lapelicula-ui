using System.Collections.Generic;

namespace LaPelicula.UI.Shared;


public class UserPreferences
{
    public double Action { get; init; }
    public double Adventure { get; init; }
    public double Animation { get; init; }
    public double Kids { get; init; }
    public double Musical { get; init; }
    public double Comedy { get; init; }
    public double Crime { get; init; }
    public double Documentary { get; init; }
    public double Drama { get; init; }
    public double Fantasy { get; init; }
    public double FilmNoir { get; init; }
    public double Horror { get; init; }
    public double Mystery { get; init; }
    public double Romance { get; init; }
    public double Scifi { get; init; }
    public double Thriller { get; init; }
    public double War { get; init; }
    public double Western { get; init; }

    
    public Dictionary<string, double> ToDictionary()
    {
        return new Dictionary<string, double>()
        {
            { GetKeyByPropertyName(nameof(Action)) , Action },
            { GetKeyByPropertyName(nameof(Adventure)), Adventure },
            { GetKeyByPropertyName(nameof(Animation)), Animation },
            { GetKeyByPropertyName(nameof(Comedy)), Comedy },
            { GetKeyByPropertyName(nameof(Crime)), Crime },
            { GetKeyByPropertyName(nameof(Documentary)), Documentary },
            { GetKeyByPropertyName(nameof(Drama)), Drama },
            { GetKeyByPropertyName(nameof(Fantasy)), Fantasy },
            { GetKeyByPropertyName(nameof(FilmNoir)), FilmNoir },
            { GetKeyByPropertyName(nameof(Horror)), Horror },
            { GetKeyByPropertyName(nameof(Kids)), Kids },
            { GetKeyByPropertyName(nameof(Musical)), Musical },
            { GetKeyByPropertyName(nameof(Mystery)), Mystery },
            { GetKeyByPropertyName(nameof(Romance)), Romance },
            { GetKeyByPropertyName(nameof(Scifi)), Scifi },
            { GetKeyByPropertyName(nameof(Thriller)), Thriller },
            { GetKeyByPropertyName(nameof(War)), War },
            { GetKeyByPropertyName(nameof(Western)), Western },
        };
    }
    
    public static UserPreferences FromDictionary(IDictionary<string, double> dictionary)
    {
        var dict = dictionary.ToDictionary();
        return new UserPreferences
        {
            Action = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Action)), 0.0),
            Adventure = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Adventure)), 0.0),
            Animation = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Animation)), 0.0),
            Comedy = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Comedy)), 0.0),
            Crime = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Crime)), 0.0),
            Documentary = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Documentary)), 0.0),
            Drama = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Drama)), 0.0),
            Fantasy = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Fantasy)), 0.0),
            FilmNoir = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(FilmNoir)), 0.0),
            Horror = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Horror)), 0.0),
            Kids = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Kids)), 0.0),
            Musical = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Musical)), 0.0),
            Mystery = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Mystery)), 0.0),
            Romance = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Romance)), 0.0),
            Scifi = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Scifi)), 0.0),
            Thriller = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Thriller)), 0.0),
            War = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(War)), 0.0),
            Western = dict.GetValueOrDefault(GetKeyByPropertyName(nameof(Western)), 0.0),
        };
    }


    private static readonly Dictionary<string, string> _propertyToKeyMapping = new()
    {
        { nameof(Action), "action" },
        { nameof(Adventure), "adventure" },
        { nameof(Animation), "animation" },
        { nameof(Comedy), "comedy" },
        { nameof(Crime), "crime" },
        { nameof(Documentary), "documentary" },
        { nameof(Drama), "drama" },
        { nameof(Fantasy), "fantasy" },
        { nameof(FilmNoir), "film_noir" },
        { nameof(Horror), "horror" },
        { nameof(Kids), "kids" },
        { nameof(Musical), "musical" },
        { nameof(Mystery), "mystery" },
        { nameof(Romance), "romance" },
        { nameof(Scifi), "sci_fi" },
        { nameof(Thriller), "thriller" },
        { nameof(War), "war" },
        { nameof(Western), "western" }
    };
    private static string GetKeyByPropertyName(string propertyName)
    {
        return _propertyToKeyMapping[propertyName];
    }
    
    public static string[] GetAllGenreKeys()
    {
        return _propertyToKeyMapping.Values.Order().ToArray();
    }
    
    
}