using System;
using System.Collections.Generic;
using System.Linq;
using Modbus.Data;
using Modbus.Message;

namespace Modbus.IntegrationTests.CustomMessages;

public class CustomReadHoldingRegistersResponse : IModbusMessage
{
    private RegisterCollection _data;

    public ushort[] Data => _data.ToArray();

    public byte[] MessageFrame
    {
        get
        {
            List<byte> frame = new();
            frame.Add(SlaveAddress);
            frame.AddRange(ProtocolDataUnit);

            return frame.ToArray();
        }
    }

    public byte[] ProtocolDataUnit
    {
        get
        {
            List<byte> pdu = new();

            pdu.Add(FunctionCode);
            pdu.Add(ByteCount);
            pdu.AddRange(_data.NetworkBytes);

            return pdu.ToArray();
        }
    }

    public ushort TransactionId { get; set; }

    public byte FunctionCode { get; set; }

    public byte SlaveAddress { get; set; }

    public byte ByteCount { get; set; }

    public void Initialize(byte[] frame)
    {
        ArgumentNullException.ThrowIfNull(frame);

        if (frame.Length < 3 || frame.Length < 3 + frame[2])
            throw new ArgumentException("Message frame does not contain enough bytes.", nameof(frame));

        SlaveAddress = frame[0];
        FunctionCode = frame[1];
        ByteCount = frame[2];
        _data = new RegisterCollection(frame.Skip(3).Take(ByteCount).ToArray());
    }
}