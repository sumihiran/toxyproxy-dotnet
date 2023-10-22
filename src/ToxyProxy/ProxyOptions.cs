namespace DotNet.ToxyProxy;

/// <summary>
/// Represents the options for configuring a Proxy instance.
/// </summary>
/// <param name="Name">The name of the proxy.</param>
/// <param name="Listen">The listener configuration.</param>
/// <param name="Upstream">The upstream configuration.</param>
/// <param name="Enabled">A value indicating whether the proxy is enabled.</param>
public record ProxyOptions(string Name, Listener Listen, Upstream Upstream, bool Enabled);
