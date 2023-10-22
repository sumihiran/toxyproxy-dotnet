namespace DotNet.ToxyProxy;

using System;
using System.Threading.Tasks;

/// <summary>
/// Represents a Proxy object configured with specified options and an associated ToxyProxy client.
/// </summary>
public class Proxy : IProxy
{
    private readonly IToxyProxyClient toxyProxyClient;
    private ProxyOptions options;
    private bool isDeleted;

    /// <summary>
    /// Initializes a new instance of the <see cref="Proxy"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the proxy.</param>
    /// <param name="toxyProxyClient">The ToxyProxy client to interact with.</param>
    public Proxy(ProxyOptions options, IToxyProxyClient toxyProxyClient)
    {
        this.options = options;
        this.toxyProxyClient = toxyProxyClient;
    }

    /// <summary>
    /// Gets the name of the proxy.
    /// </summary>
    public string Name => this.options.Name;

    /// <summary>
    /// Gets the listener.
    /// </summary>
    public Listener Listen => this.options.Listen;

    /// <summary>
    /// Gets the upstream.
    /// </summary>
    public Upstream Upstream => this.options.Upstream;

    /// <summary>
    /// Gets a value indicating whether the proxy is enabled.
    /// </summary>
    /// <value>
    /// <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </value>
    public bool IsEnabled => this.options.Enabled;

    /// <summary>
    /// Asynchronously enables the proxy.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the proxy is already deleted.</exception>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task EnableAsync()
    {
        this.ThrowIfDeleted();

        var newProxy = await this.toxyProxyClient
            .UpdateAsync(this.Name, this.options with { Enabled = true })
            .ConfigureAwait(false);

        this.options = MapToOptions(newProxy);
    }

    /// <inheritdoc />
    public async Task DisableAsync()
    {
        this.ThrowIfDeleted();

        var newProxy = await this.toxyProxyClient
            .UpdateAsync(this.Name, this.options with { Enabled = false })
            .ConfigureAwait(false);

        this.options = MapToOptions(newProxy);
    }

    /// <inheritdoc />
    public async Task DeleteAsync()
    {
        await this.toxyProxyClient.DeleteAsync(this.Name).ConfigureAwait(false);
        this.isDeleted = true;
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (this.isDeleted)
        {
            return;
        }

        GC.SuppressFinalize(this);
        await this.DeleteAsync().ConfigureAwait(false);
    }

    private static ProxyOptions MapToOptions(IProxy proxy)
        => new(proxy.Name, proxy.Listen, proxy.Upstream, proxy.IsEnabled);

    /// <summary>
    /// Throws an exception if the proxy is deleted.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the proxy is already deleted.</exception>
    private void ThrowIfDeleted()
    {
        if (this.isDeleted)
        {
            throw new InvalidOperationException("Cannot perform this operation on a deleted proxy.");
        }
    }
}
