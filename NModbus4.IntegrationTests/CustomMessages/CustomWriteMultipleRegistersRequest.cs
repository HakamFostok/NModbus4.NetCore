using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Modbus.Data;
using Modbus.Message;

namespace Modbus.IntegrationTests.CustomMessages;

public class CustomWriteMultipleRegistersRequest : IModbusMessage
{
    public CustomWriteMultipleRegistersRequest(byte functionCode, byte slaveAddress, ushort startAddress, RegisterCollection data)
    {
        FunctionCode = functionCode;
        SlaveAddress = slaveAddress;
        StartAddress = startAddress;
        NumberOfPoints = (ushort)data.Count;
        ByteCount = data.ByteCount;
        Data = data;
    }

    public byte[] MessageFrame
    {
        get
        {
            List<byte> frame = new()
            {
                SlaveAddress
            };
            frame.AddRange(ProtocolDataUnit);

            return frame.ToArray();
        }
    }

    public byte[] ProtocolDataUnit
    {
        get
        {
            List<byte> pdu = new()
            {
                FunctionCode
            };
            pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)StartAddress)));
            pdu.AddRange(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)NumberOfPoints)));
            pdu.Add(ByteCount);
            pdu.AddRange(Data.NetworkBytes);

            return pdu.ToArray();
        }
    }

    public ushort TransactionId { get; set; }

    public byte FunctionCode { get; set; }

    public byte SlaveAddress { get; set; }

    public ushort StartAddress { get; set; }

    public ushort NumberOfPoints { get; set; }

    public byte ByteCount { get; set; }

    public RegisterCollection Data { get; set; }

    public void Initialize(byte[] frame)
    {
        ArgumentNullException.ThrowIfNull(frame);

        if (frame.Length < 7 || frame.Length < 7 + frame[6])
            throw new FormatException("Message frame does not contain enough bytes.");

        SlaveAddress = frame[0];
        FunctionCode = frame[1];
        StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
        NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
        ByteCount = frame[6];
        Data = new RegisterCollection(frame.Skip(7).Take(ByteCount).ToArray());
    }
}