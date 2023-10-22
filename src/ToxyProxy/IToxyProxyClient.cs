namespace DotNet.ToxyProxy;

using System.Threading.Tasks;

/// <summary>
/// Provides the interface for interacting with ToxyProxy.
/// </summary>
public interface IToxyProxyClient
{
    /// <summary>
    /// Updates a proxy with the given name and options.
    /// </summary>
    /// <param name="name">The name of the proxy to update.</param>
    /// <param name="options">A <see cref="ProxyOptions"/> object containing the new settings for the proxy.</param>
    /// <returns>A <see cref="Task"/> of <see cref="Proxy"/> representing the updated proxy.</returns>
    /// <exception cref="ArgumentException">Thrown when the name or options are invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the update operation fails.</exception>
    Task<IProxy> UpdateAsync(string name, ProxyOptions options);

    /// <summary>
    /// Deletes a proxy with the given name.
    /// </summary>
    /// <param name="name">The name of the proxy to delete.</param>
    /// <returns>A <see cref="Task"/> of <see cref="Proxy"/> representing the deleted proxy.</returns>
    /// <exception cref="ArgumentException">Thrown when the name is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the delete operation fails.</exception>
    Task DeleteAsync(string name);
}
