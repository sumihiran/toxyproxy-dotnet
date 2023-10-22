namespace DotNet.ToxyProxy.Test;

using Moq;
using Xunit;

public class ProxyTests
{
    private readonly Mock<IToxyProxyClient> mockClient = new();
    private readonly ProxyOptions proxyOptions
        = new("proxy-name", new Listener("localhost", 8080), new Upstream("localhost", 80), true);

    [Fact]
    public async Task EnableAsync_ShouldEnableProxy()
    {
        // Arrange
        var options = this.proxyOptions with { Enabled = false };
        var sut = new Proxy(options, this.mockClient.Object);
        this.mockClient.Setup(client => client.UpdateAsync("proxy-name", It.Is<ProxyOptions>(o => o.Enabled)))
            .ReturnsAsync(new Proxy(options with { Enabled = true }, this.mockClient.Object));

        // Act
        await sut.EnableAsync();

        // Assert
        Assert.True(sut.IsEnabled);
    }

    [Fact]
    public async Task DisableAsync_ShouldDisableProxy()
    {
        // Arrange
        var options = this.proxyOptions with { Enabled = true };
        var sut = new Proxy(options, this.mockClient.Object);
        this.mockClient.Setup(client => client.UpdateAsync("proxy-name", It.Is<ProxyOptions>(o => !o.Enabled)))
            .ReturnsAsync(new Proxy(options with { Enabled = false }, this.mockClient.Object));

        // Act
        await sut.DisableAsync();

        // Assert
        Assert.False(sut.IsEnabled);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProxy()
    {
        // Arrange
        var proxy = new Proxy(this.proxyOptions, this.mockClient.Object);
        this.mockClient.Setup(client => client.DeleteAsync("proxy-name"))
            .Returns(Task.CompletedTask);

        // Act
        await proxy.DeleteAsync();

        // Assert
        this.mockClient.Verify(client => client.DeleteAsync("proxy-name"), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_ShouldDisposeProxy()
    {
        // Arrange
        var proxy = new Proxy(this.proxyOptions, this.mockClient.Object);
        this.mockClient.Setup(client => client.DeleteAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await proxy.DisposeAsync();

        // Assert
        this.mockClient.Verify(client => client.DeleteAsync("proxy-name"), Times.Once);
    }
}
