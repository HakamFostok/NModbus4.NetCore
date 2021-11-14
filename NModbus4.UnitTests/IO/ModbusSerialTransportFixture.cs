using System;
using System.IO;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;
using Modbus.UnitTests.Message;
using Modbus.Utility;
using Moq;
using Xunit;

namespace Modbus.UnitTests.IO;

public class ModbusSerialTransportFixture
{
    private static IStreamResource StreamResource => new Mock<IStreamResource>(MockBehavior.Strict).Object;

    [Fact]
    public void CreateResponse()
    {
        ModbusAsciiTransport? transport = new(StreamResource);
        ReadCoilsInputsResponse? expectedResponse = new(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, false, false, false, false, false, true));
        byte lrc = ModbusUtility.CalculateLrc(expectedResponse.MessageFrame);
        IModbusMessage? response = transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 2, Modbus.ReadCoils, 1, 129, lrc });

        Assert.IsType<ReadCoilsInputsResponse>(response);
        ModbusMessageFixture.AssertModbusMessagePropertiesAreEqual(expectedResponse, response);
    }

    [Fact]
    public void CreateResponseErroneousLrc()
    {
        ModbusAsciiTransport? transport = new(StreamResource) { CheckFrame = true };
        byte[]? frame = new byte[] { 19, Modbus.ReadCoils, 0, 0, 0, 2, 115 };

        Assert.Throws<IOException>(
            () => transport.CreateResponse<ReadCoilsInputsResponse>(frame));
    }

    [Fact]
    public void CreateResponseErroneousLrcDoNotCheckFrame()
    {
        ModbusAsciiTransport? transport = new(StreamResource) { CheckFrame = false };

        transport.CreateResponse<ReadCoilsInputsResponse>(new byte[] { 19, Modbus.ReadCoils, 0, 0, 0, 2, 115 });
    }

    /// <summary>
    /// When using the serial RTU protocol the beginning of the message could get mangled leading to an unsupported message type.
    /// We want to be sure to try the message again so clear the RX buffer and try again.
    /// </summary>
    [Fact]
    public void UnicastMessage_PurgeReceiveBuffer()
    {
        Mock<IStreamResource>? mock = new(MockBehavior.Strict);
        IStreamResource serialResource = mock.Object;
        ModbusRtuTransport? transport = new(serialResource);

        mock.Setup(s => s.DiscardInBuffer());
        mock.Setup(s => s.Write(It.IsAny<byte[]>(), 0, 0));

        serialResource.DiscardInBuffer();
        serialResource.Write(null, 0, 0);

        // mangled response
        mock.Setup(s => s.Read(It.Is<byte[]>(x => x.Length == 4), 0, 4)).Returns(4);

        serialResource.DiscardInBuffer();
        serialResource.Write(null, 0, 0);

        // normal response
        ReadCoilsInputsResponse? response = new(Modbus.ReadCoils, 2, 1, new DiscreteCollection(true, false, true, false, false, false, false, false));

        // write request
        mock.Setup(s => s.Write(It.Is<byte[]>(x => x.Length == 8), 0, 8));

        // read header
        mock.Setup(s => s.Read(It.Is<byte[]>(x => x.Length == 4), 0, 4))
            .Returns((byte[] buf, int offset, int count) =>
            {
                Array.Copy(response.MessageFrame, 0, buf, 0, 4);
                return 4;
            });

        // read remainder
        mock.Setup(s => s.Read(It.Is<byte[]>(x => x.Length == 2), 0, 2))
            .Returns((byte[] buf, int offset, int count) =>
            {
                Array.Copy(ModbusUtility.CalculateCrc(response.MessageFrame), 0, buf, 0, 2);
                return 2;
            });

        ReadCoilsInputsRequest? request = new(Modbus.ReadCoils, 2, 3, 4);
        ReadCoilsInputsResponse? actualResponse = transport.UnicastMessage<ReadCoilsInputsResponse>(request);

        ModbusMessageFixture.AssertModbusMessagePropertiesAreEqual(response, actualResponse);
        mock.VerifyAll();
    }
}