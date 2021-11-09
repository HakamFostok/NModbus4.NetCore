using System.IO;
using Xunit;

namespace Modbus.UnitTests;

public class InvalidModbusRequestExceptionFixture
{
    [Fact]
    public void ConstructorWithExceptionCode()
    {
        InvalidModbusRequestException? e = new(Modbus.SlaveDeviceBusy);
        Assert.Equal($"Modbus exception code {Modbus.SlaveDeviceBusy}.", e.Message);
        Assert.Equal(Modbus.SlaveDeviceBusy, e.ExceptionCode);
        Assert.Null(e.InnerException);
    }

    [Fact]
    public void ConstructorWithExceptionCodeAndInnerException()
    {
        IOException? inner = new("Bar");
        InvalidModbusRequestException? e = new(42, inner);
        Assert.Equal("Modbus exception code 42.", e.Message);
        Assert.Equal(42, e.ExceptionCode);
        Assert.Same(inner, e.InnerException);
    }

    [Fact]
    public void ConstructorWithMessageAndExceptionCode()
    {
        InvalidModbusRequestException? e = new("Hello World", Modbus.IllegalFunction);
        Assert.Equal("Hello World", e.Message);
        Assert.Equal(Modbus.IllegalFunction, e.ExceptionCode);
        Assert.Null(e.InnerException);
    }

    [Fact]
    public void ConstructorWithCustomMessageAndSlaveExceptionResponse()
    {
        IOException? inner = new("Bar");
        InvalidModbusRequestException? e = new("Hello World", Modbus.IllegalDataAddress, inner);
        Assert.Equal("Hello World", e.Message);
        Assert.Equal(Modbus.IllegalDataAddress, e.ExceptionCode);
        Assert.Same(inner, e.InnerException);
    }
}
