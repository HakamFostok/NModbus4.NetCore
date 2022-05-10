using System.Diagnostics;
using System.Net;
using Modbus.Message;
using Modbus.Unme.Common;

namespace Modbus.IO;

/// <summary>
///     Transport for Internet protocols.
///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
/// </summary>
internal class ModbusIpTransport : ModbusTransport
{
    private static readonly object _transactionIdLock = new();
    private ushort _transactionId;

    internal ModbusIpTransport(IStreamResource streamResource)
        : base(streamResource)
    {
        Debug.Assert(streamResource is not null, "Argument streamResource cannot be null.");
    }

    internal static byte[] ReadRequestResponse(IStreamResource streamResource)
    {
        // read header
        byte[] mbapHeader = new byte[6];
        int numBytesRead = 0;

        while (numBytesRead != 6)
        {
            int bRead = streamResource.Read(mbapHeader, numBytesRead, 6 - numBytesRead);

            if (bRead == 0)
            {
                throw new IOException("Read resulted in 0 bytes returned.");
            }

            numBytesRead += bRead;
        }

        Debug.WriteLine($"MBAP header: {string.Join(", ", mbapHeader)}");
        ushort frameLength = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(mbapHeader, 4));
        Debug.WriteLine($"{frameLength} bytes in PDU.");

        // read message
        byte[] messageFrame = new byte[frameLength];
        numBytesRead = 0;

        while (numBytesRead != frameLength)
        {
            int bRead = streamResource.Read(messageFrame, numBytesRead, frameLength - numBytesRead);

            if (bRead == 0)
            {
                throw new IOException("Read resulted in 0 bytes returned.");
            }

            numBytesRead += bRead;
        }

        Debug.WriteLine($"PDU: {frameLength}");
        byte[] frame = mbapHeader.Concat(messageFrame).ToArray();
        Debug.WriteLine($"RX: {string.Join(", ", frame)}");

        return frame;
    }

    internal static byte[] GetMbapHeader(IModbusMessage message)
    {
        byte[] transactionId = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)message.TransactionId));
        byte[] length = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)(message.ProtocolDataUnit.Length + 1)));

        MemoryStream stream = new(7);
        stream.Write(transactionId, 0, transactionId.Length);
        stream.WriteByte(0);
        stream.WriteByte(0);
        stream.Write(length, 0, length.Length);
        stream.WriteByte(message.SlaveAddress);

        return stream.ToArray();
    }

    /// <summary>
    ///     Create a new transaction ID.
    /// </summary>
    internal virtual ushort GetNewTransactionId()
    {
        lock (_transactionIdLock)
        {
            _transactionId = _transactionId == ushort.MaxValue ? (ushort)1 : ++_transactionId;
        }

        return _transactionId;
    }

    internal IModbusMessage CreateMessageAndInitializeTransactionId<T>(byte[] fullFrame)
        where T : IModbusMessage, new()
    {
        byte[] mbapHeader = fullFrame.Slice(0, 6).ToArray();
        byte[] messageFrame = fullFrame.Slice(6, fullFrame.Length - 6).ToArray();

        IModbusMessage response = CreateResponse<T>(messageFrame);
        response.TransactionId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(mbapHeader, 0));

        return response;
    }

    internal override byte[] BuildMessageFrame(IModbusMessage message)
    {
        byte[] header = GetMbapHeader(message);
        byte[] pdu = message.ProtocolDataUnit;
        MemoryStream messageBody = new(header.Length + pdu.Length);

        messageBody.Write(header, 0, header.Length);
        messageBody.Write(pdu, 0, pdu.Length);

        return messageBody.ToArray();
    }

    internal override void Write(IModbusMessage message)
    {
        message.TransactionId = GetNewTransactionId();
        byte[] frame = BuildMessageFrame(message);
        Debug.WriteLine($"TX: {string.Join(", ", frame)}");
        StreamResource.Write(frame, 0, frame.Length);
    }

    internal override byte[] ReadRequest() =>
        ReadRequestResponse(StreamResource);

    internal override IModbusMessage ReadResponse<T>() =>
        CreateMessageAndInitializeTransactionId<T>(ReadRequestResponse(StreamResource));

    internal override void OnValidateResponse(IModbusMessage request, IModbusMessage response)
    {
        if (request.TransactionId != response.TransactionId)
        {
            string msg = $"Response was not of expected transaction ID. Expected {request.TransactionId}, received {response.TransactionId}.";
            throw new IOException(msg);
        }
    }

    internal override bool OnShouldRetryResponse(IModbusMessage request, IModbusMessage response)
    {
        if (request.TransactionId > response.TransactionId && request.TransactionId - response.TransactionId < RetryOnOldResponseThreshold)
        {
            // This response was from a previous request
            return true;
        }

        return base.OnShouldRetryResponse(request, response);
    }
}