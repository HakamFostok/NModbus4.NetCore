﻿using System.Diagnostics;
using System.Net;
using Modbus.Data;
using Modbus.Unme.Common;

namespace Modbus.Message;

public class WriteSingleCoilRequestResponse : AbstractModbusMessageWithData<RegisterCollection>, IModbusRequest
{
    public WriteSingleCoilRequestResponse()
    {
    }

    public WriteSingleCoilRequestResponse(byte slaveAddress, ushort startAddress, bool coilState)
        : base(slaveAddress, Modbus.WriteSingleCoil)
    {
        StartAddress = startAddress;
        Data = new RegisterCollection(coilState ? Modbus.CoilOn : Modbus.CoilOff);
    }

    public override int MinimumFrameSize => 6;

    public ushort StartAddress
    {
        get => MessageImpl.StartAddress.Value;
        set => MessageImpl.StartAddress = value;
    }

    public override string ToString()
    {
        Debug.Assert(Data != null, "Argument Data cannot be null.");
        Debug.Assert(Data.Count() == 1, "Data should have a count of 1.");

        string msg = $"Write single coil {(Data.First() == Modbus.CoilOn ? 1 : 0)} at address {StartAddress}.";
        return msg;
    }

    public void ValidateResponse(IModbusMessage response)
    {
        WriteSingleCoilRequestResponse? typedResponse = (WriteSingleCoilRequestResponse)response;

        if (StartAddress != typedResponse.StartAddress)
        {
            string msg = $"Unexpected start address in response. Expected {StartAddress}, received {typedResponse.StartAddress}.";
            throw new IOException(msg);
        }

        if (Data.First() != typedResponse.Data.First())
        {
            string msg = $"Unexpected data in response. Expected {Data.First()}, received {typedResponse.Data.First()}.";
            throw new IOException(msg);
        }
    }

    protected override void InitializeUnique(byte[] frame)
    {
        StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
        Data = new RegisterCollection(frame.Slice(4, 2).ToArray());
    }
}