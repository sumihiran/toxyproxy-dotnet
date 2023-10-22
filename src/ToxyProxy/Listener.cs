namespace DotNet.ToxyProxy;

using System.Net;

/// <summary>
/// Represents a Listener DNS endpoint with a host name and a port number.
/// </summary>
public class Listener : DnsEndPoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Listener"/> class.
    /// </summary>
    /// <param name="host">The host name.</param>
    /// <param name="port">The port number.</param>
    public Listener(string host, int port)
        : base(host, port)
    {
    }

    /// <summary>
    /// Converts a string to a <see cref="Listener"/> object.
    /// </summary>
    /// <param name="endpoint">A string representing the endpoint in the format host:port.</param>
    /// <returns>A <see cref="Listener"/> object.</returns>
    public static implicit operator Listener(string endpoint)
    {
        var parts = endpoint.Split(':');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var port))
        {
            throw new FormatException("Invalid listener endpoint format. Expected format: host:port");
        }

        return new Listener(parts[0], port);
    }

    /// <summary>
    /// Converts a <see cref="Listener"/> object to a string.
    /// </summary>
    /// <param name="listener">A <see cref="Listener"/> object.</param>
    /// <returns>A string representing the endpoint in the format host:port.</returns>
    public static implicit operator string(Listener listener) => listener.ToString();

    /// <inheritdoc />
    public override string ToString() => $"{this.Host}:{this.Port}";
}
