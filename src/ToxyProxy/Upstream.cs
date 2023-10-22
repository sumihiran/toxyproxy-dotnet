namespace DotNet.ToxyProxy;

using System.Net;

/// <summary>
/// Represents a Upstream DNS endpoint with a host name and a port number.
/// </summary>
public class Upstream : DnsEndPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Upstream"/> class.
    /// </summary>
    /// <param name="host">The host name.</param>
    /// <param name="port">The port number.</param>
    public Upstream(string host, int port)
        : base(host, port)
    {
    }

    /// <summary>
    /// Converts a string to a <see cref="Upstream"/> object.
    /// </summary>
    /// <param name="endpoint">A string representing the endpoint in the format host:port.</param>
    /// <returns>A <see cref="Upstream"/> object.</returns>
    public static implicit operator Upstream(string endpoint)
    {
        var parts = endpoint.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var port))
        {
            throw new FormatException("Invalid upstream endpoint format. Expected format: host:port");
        }

        return new Upstream(parts[0], port);
    }

    /// <summary>
    /// Converts a <see cref="Upstream"/> object to a string.
    /// </summary>
    /// <param name="upstream">A <see cref="Upstream"/> object.</param>
    /// <returns>A string representing the endpoint in the format host:port.</returns>
    public static implicit operator string(Upstream upstream) => upstream.ToString();

    /// <inheritdoc />
    public override string ToString() => $"{this.Host}:{this.Port}";
}
