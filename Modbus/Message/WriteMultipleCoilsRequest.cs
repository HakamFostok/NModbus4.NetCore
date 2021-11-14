﻿using System.Net;
using Modbus.Data;
using Modbus.Unme.Common;

namespace Modbus.Message;

/// <summary>
///     Write Multiple Coils request.
/// </summary>
public class WriteMultipleCoilsRequest : AbstractModbusMessageWithData<DiscreteCollection>, IModbusRequest
{
    /// <summary>
    ///     Write Multiple Coils request.
    /// </summary>
    public WriteMultipleCoilsRequest()
    {
    }

    /// <summary>
    ///     Write Multiple Coils request.
    /// </summary>
    public WriteMultipleCoilsRequest(byte slaveAddress, ushort startAddress, DiscreteCollection data)
        : base(slaveAddress, Modbus.WriteMultipleCoils)
    {
        StartAddress = startAddress;
        NumberOfPoints = (ushort)data.Count;
        ByteCount = (byte)((data.Count + 7) / 8);
        Data = data;
    }

    public byte ByteCount
    {
        get => MessageImpl.ByteCount.Value;
        set => MessageImpl.ByteCount = value;
    }

    public ushort NumberOfPoints
    {
        get => MessageImpl.NumberOfPoints.Value;

        set
        {
            if (value > Modbus.MaximumDiscreteRequestResponseSize)
            {
                string msg = $"Maximum amount of data {Modbus.MaximumDiscreteRequestResponseSize} coils.";
                throw new ArgumentOutOfRangeException("NumberOfPoints", msg);
            }

            MessageImpl.NumberOfPoints = value;
        }
    }

    public ushort StartAddress
    {
        get => MessageImpl.StartAddress.Value;
        set => MessageImpl.StartAddress = value;
    }

    public override int MinimumFrameSize => 7;

    public override string ToString() =>
        $"Write {NumberOfPoints} coils starting at address {StartAddress}.";

    public void ValidateResponse(IModbusMessage response)
    {
        WriteMultipleCoilsResponse? typedResponse = (WriteMultipleCoilsResponse)response;

        if (StartAddress != typedResponse.StartAddress)
        {
            string msg = $"Unexpected start address in response. Expected {StartAddress}, received {typedResponse.StartAddress}.";
            throw new IOException(msg);
        }

        if (NumberOfPoints != typedResponse.NumberOfPoints)
        {
            string msg = $"Unexpected number of points in response. Expected {NumberOfPoints}, received {typedResponse.NumberOfPoints}.";
            throw new IOException(msg);
        }
    }

    protected override void InitializeUnique(byte[] frame)
    {
        if (frame.Length < MinimumFrameSize + frame[6])
        {
            throw new FormatException("Message frame does not contain enough bytes.");
        }

        StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
        NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
        ByteCount = frame[6];
        Data = new DiscreteCollection(frame.Slice(7, ByteCount).ToArray());
    }
}
