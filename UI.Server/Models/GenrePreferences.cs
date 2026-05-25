using System;
using System.Text.Json.Serialization;

namespace LaPelicula.UI.Server.Models;

public class GenrePreferences
{
    [JsonPropertyName("action")]
    public double Action { get; init; }

    [JsonPropertyName("adventure")]
    public double Adventure { get; init; }

    [JsonPropertyName("animation")]
    public double Animation { get; init; }

    [JsonPropertyName("kids")]
    public double Kids { get; init; }

    [JsonPropertyName("musical")]
    public double Musical { get; init; }

    [JsonPropertyName("comedy")]
    public double Comedy { get; init; }

    [JsonPropertyName("crime")]
    public double Crime { get; init; }

    [JsonPropertyName("documentary")]
    public double Documentary { get; init; }

    [JsonPropertyName("drama")]
    public double Drama { get; init; }

    [JsonPropertyName("fantasy")]
    public double Fantasy { get; init; }

    [JsonPropertyName("film_noir")]
    public double FilmNoir { get; init; }

    [JsonPropertyName("horror")]
    public double Horror { get; init; }

    [JsonPropertyName("mystery")]
    public double Mystery { get; init; }

    [JsonPropertyName("romance")]
    public double Romance { get; init; }

    [JsonPropertyName("sci_fi")]
    public double Scifi { get; init; }

    [JsonPropertyName("thriller")]
    public double Thriller { get; init; }

    [JsonPropertyName("war")]
    public double War { get; init; }
    
    [JsonPropertyName("western")]
    public double Western { get; init; }
}
