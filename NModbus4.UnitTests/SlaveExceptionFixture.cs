using System;
using System.IO;
using Modbus.Message;
using Xunit;

namespace Modbus.UnitTests;

public class SlaveExceptionFixture
{
    [Fact]
    public void EmptyConstructor()
    {
        SlaveException? e = new();
        Assert.Equal($"Exception of type '{typeof(SlaveException).FullName}' was thrown.", e.Message);
        Assert.Equal(0, e.SlaveAddress);
        Assert.Equal(0, e.FunctionCode);
        Assert.Equal(0, e.SlaveExceptionCode);
        Assert.Null(e.InnerException);
    }

    [Fact]
    public void ConstructorWithMessage()
    {
        SlaveException? e = new("Hello World");
        Assert.Equal("Hello World", e.Message);
        Assert.Equal(0, e.SlaveAddress);
        Assert.Equal(0, e.FunctionCode);
        Assert.Equal(0, e.SlaveExceptionCode);
        Assert.Null(e.InnerException);
    }

    [Fact]
    public void ConstructorWithMessageAndInnerException()
    {
        IOException? inner = new("Bar");
        SlaveException? e = new("Foo", inner);
        Assert.Equal("Foo", e.Message);
        Assert.Same(inner, e.InnerException);
        Assert.Equal(0, e.SlaveAddress);
        Assert.Equal(0, e.FunctionCode);
        Assert.Equal(0, e.SlaveExceptionCode);
    }

    [Fact]
    public void ConstructorWithSlaveExceptionResponse()
    {
        SlaveExceptionResponse? response = new(12, Modbus.ReadCoils, 1);
        SlaveException? e = new(response);

        Assert.Equal(12, e.SlaveAddress);
        Assert.Equal(Modbus.ReadCoils, e.FunctionCode);
        Assert.Equal(1, e.SlaveExceptionCode);
        Assert.Null(e.InnerException);

        Assert.Equal(
            $@"Exception of type '{typeof(SlaveException).FullName}' was thrown.{Environment.NewLine}Function Code: {response.FunctionCode}{Environment.NewLine}Exception Code: {response.SlaveExceptionCode} - {Resources.IllegalFunction}",
            e.Message);
    }

    [Fact]
    public void ConstructorWithCustomMessageAndSlaveExceptionResponse()
    {
        SlaveExceptionResponse? response = new(12, Modbus.ReadCoils, 2);
        string customMessage = "custom message";
        SlaveException? e = new(customMessage, response);

        Assert.Equal(12, e.SlaveAddress);
        Assert.Equal(Modbus.ReadCoils, e.FunctionCode);
        Assert.Equal(2, e.SlaveExceptionCode);
        Assert.Null(e.InnerException);

        Assert.Equal(
            $@"{customMessage}{Environment.NewLine}Function Code: {response.FunctionCode}{Environment.NewLine}Exception Code: {response.SlaveExceptionCode} - {Resources.IllegalDataAddress}",
            e.Message);
    }
}
