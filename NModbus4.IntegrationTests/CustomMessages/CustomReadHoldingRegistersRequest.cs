using System;
using System.Collections.Generic;
using System.Net;
using Modbus.Message;

namespace Modbus.IntegrationTests.CustomMessages;

public class CustomReadHoldingRegistersRequest : IModbusMessage
{
    public CustomReadHoldingRegistersRequest(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        FunctionCode = functionCode;
        SlaveAddress = slaveAddress;
        StartAddress = startAddress;
        NumberOfPoints = numberOfPoints;
    }

    public byte[] MessageFrame
    {
        get
        {
            List<byte> frame = new() { SlaveAddress };
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

            return pdu.ToArray();
        }
    }

    public ushort TransactionId { get; set; }

    public byte FunctionCode { get; set; }

    public byte SlaveAddress { get; set; }

    public ushort StartAddress { get; set; }

    public ushort NumberOfPoints { get; set; }

    public void Initialize(byte[] frame)
    {
        ArgumentNullException.ThrowIfNull(frame);

        if (frame.Length != 6)
            throw new ArgumentException("Invalid frame.", nameof(frame));

        SlaveAddress = frame[0];
        FunctionCode = frame[1];
        StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
        NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
    }
}