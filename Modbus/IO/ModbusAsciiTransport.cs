using System.Diagnostics;
using System.IO;
using System.Text;

using Modbus.Message;
using Modbus.Utility;

namespace Modbus.IO
{
    /// <summary>
    ///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
    /// </summary>
    internal class ModbusAsciiTransport : ModbusSerialTransport
    {
        internal ModbusAsciiTransport(IStreamResource streamResource)
            : base(streamResource)
        {
            Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");
        }

        internal override byte[] BuildMessageFrame(IModbusMessage message)
        {
            byte[]? msgFrame = message.MessageFrame;

            byte[]? msgFrameAscii = ModbusUtility.GetAsciiBytes(msgFrame);
            byte[]? lrcAscii = ModbusUtility.GetAsciiBytes(ModbusUtility.CalculateLrc(msgFrame));
            byte[]? nlAscii = Encoding.UTF8.GetBytes(Modbus.NewLine.ToCharArray());

            MemoryStream? frame = new MemoryStream(1 + msgFrameAscii.Length + lrcAscii.Length + nlAscii.Length);
            frame.WriteByte((byte)':');
            frame.Write(msgFrameAscii, 0, msgFrameAscii.Length);
            frame.Write(lrcAscii, 0, lrcAscii.Length);
            frame.Write(nlAscii, 0, nlAscii.Length);

            return frame.ToArray();
        }

        internal override bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame) =>
            ModbusUtility.CalculateLrc(message.MessageFrame) == messageFrame[^1];

        internal override byte[] ReadRequest() =>
            ReadRequestResponse();

        internal override IModbusMessage ReadResponse<T>() =>
            CreateResponse<T>(ReadRequestResponse());

        internal byte[] ReadRequestResponse()
        {
            // read message frame, removing frame start ':'
            string frameHex = StreamResourceUtility.ReadLine(StreamResource)[1..];

            // convert hex to bytes
            byte[] frame = ModbusUtility.HexToBytes(frameHex);
            Debug.WriteLine($"RX: {string.Join(", ", frame)}");

            if (frame.Length < 3)
            {
                throw new IOException("Premature end of stream, message truncated.");
            }

            return frame;
        }
    }
}
