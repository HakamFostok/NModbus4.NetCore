using System;
using System.IO;
using Modbus.Message;
using Xunit;

namespace Modbus.UnitTests
{
    public class SlaveExceptionFixture
    {
        [Fact]
        public void EmptyConstructor()
        {
            var e = new SlaveException();
            Assert.Equal($"Exception of type '{typeof(SlaveException).FullName}' was thrown.", e.Message);
            Assert.Equal(0, e.SlaveAddress);
            Assert.Equal(0, e.FunctionCode);
            Assert.Equal(0, e.SlaveExceptionCode);
            Assert.Null(e.InnerException);
        }

        [Fact]
        public void ConstructorWithMessage()
        {
            var e = new SlaveException("Hello World");
            Assert.Equal("Hello World", e.Message);
            Assert.Equal(0, e.SlaveAddress);
            Assert.Equal(0, e.FunctionCode);
            Assert.Equal(0, e.SlaveExceptionCode);
            Assert.Null(e.InnerException);
        }

        [Fact]
        public void ConstructorWithMessageAndInnerException()
        {
            var inner = new IOException("Bar");
            var e = new SlaveException("Foo", inner);
            Assert.Equal("Foo", e.Message);
            Assert.Same(inner, e.InnerException);
            Assert.Equal(0, e.SlaveAddress);
            Assert.Equal(0, e.FunctionCode);
            Assert.Equal(0, e.SlaveExceptionCode);
        }

        [Fact]
        public void ConstructorWithSlaveExceptionResponse()
        {
            var response = new SlaveExceptionResponse(12, Modbus.ReadCoils, 1);
            var e = new SlaveException(response);

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
            var response = new SlaveExceptionResponse(12, Modbus.ReadCoils, 2);
            string customMessage = "custom message";
            var e = new SlaveException(customMessage, response);

            Assert.Equal(12, e.SlaveAddress);
            Assert.Equal(Modbus.ReadCoils, e.FunctionCode);
            Assert.Equal(2, e.SlaveExceptionCode);
            Assert.Null(e.InnerException);

            Assert.Equal(
                $@"{customMessage}{Environment.NewLine}Function Code: {response.FunctionCode}{Environment.NewLine}Exception Code: {response.SlaveExceptionCode} - {Resources.IllegalDataAddress}",
                e.Message);
        }
    }
}
