using System;
using Modbus.Device;
using Xunit;

namespace Modbus.UnitTests.Device;

public class TcpConnectionEventArgsFixture
{
    [Fact]
    public void TcpConnectionEventArgs_NullEndPoint() =>
        Assert.Throws<ArgumentNullException>(() => new TcpConnectionEventArgs(null));

    [Fact]
    public void TcpConnectionEventArgs_EmptyEndPoint() =>
        Assert.Throws<ArgumentException>(() => new TcpConnectionEventArgs(string.Empty));

    [Fact]
    public void TcpConnectionEventArgs()
    {
        TcpConnectionEventArgs args = new("foo");

        Assert.Equal("foo", args.EndPoint);
    }
}