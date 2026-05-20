namespace LaPelicula.UI.Server.Common;

public class RepositoryCacheConfig
{
    public TimeSpan CacheDuration { get; init; } = TimeSpan.FromMinutes(60);
}
