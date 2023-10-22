namespace DotNet.ToxyProxy;

using System;
using System.Threading.Tasks;

/// <summary>
/// Provides an interface for interacting with Proxy objects.
/// </summary>
public interface IProxy : IAsyncDisposable
{
    /// <summary>
    /// Gets the name of the proxy.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the listener settings.
    /// </summary>
    Listener Listen { get; }

    /// <summary>
    /// Gets the upstream settings.
    /// </summary>
    Upstream Upstream { get; }

    /// <summary>
    /// Gets a value indicating whether the proxy is enabled.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Enables the proxy.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the proxy is already deleted.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task EnableAsync();

    /// <summary>
    /// Disables the proxy.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the proxy is already deleted.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DisableAsync();

    /// <summary>
    /// Deletes the proxy.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task DeleteAsync();
}
