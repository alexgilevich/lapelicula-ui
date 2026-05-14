using LaPelicula.UI.Server.Models;

public class UserProfile
{
    public GenrePreferences GenrePreferences { get; init; }
    public long UserId { get; init; }
    public DateTime BirthDate { get; init; }
}