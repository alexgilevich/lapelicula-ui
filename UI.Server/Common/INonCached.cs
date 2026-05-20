namespace LaPelicula.UI.Server.Common;

/// <summary>
/// Interface indicating that the implementing service provides uncached, direct access to its data source.
/// Used by CachedDynamoDbMovieRepository to distinguish the raw DynamoDB repository from cached wrappers.
/// </summary>
public interface INonCached<out T> where T : class
{
    /// <summary>
    /// Gets the underlying wrapped service instance.
    /// </summary>
    T Instance { get; }
}
