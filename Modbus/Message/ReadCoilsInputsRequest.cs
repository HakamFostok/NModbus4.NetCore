﻿using System;
using System.IO;
using System.Net;

namespace Modbus.Message;

public class ReadCoilsInputsRequest : AbstractModbusMessage, IModbusRequest
{
    public ReadCoilsInputsRequest()
    {
    }

    public ReadCoilsInputsRequest(byte functionCode, byte slaveAddress, ushort startAddress, ushort numberOfPoints)
        : base(slaveAddress, functionCode)
    {
        StartAddress = startAddress;
        NumberOfPoints = numberOfPoints;
    }

    public ushort StartAddress
    {
        get => MessageImpl.StartAddress.Value;
        set => MessageImpl.StartAddress = value;
    }

    public override int MinimumFrameSize => 6;

    public ushort NumberOfPoints
    {
        get => MessageImpl.NumberOfPoints.Value;
        set
        {
            if (value > Modbus.MaximumDiscreteRequestResponseSize)
            {
                string msg = $"Maximum amount of data {Modbus.MaximumDiscreteRequestResponseSize} coils.";
                throw new ArgumentOutOfRangeException(nameof(NumberOfPoints), msg);
            }

            MessageImpl.NumberOfPoints = value;
        }
    }

    public override string ToString() =>
        $"Read {NumberOfPoints} {(FunctionCode == Modbus.ReadCoils ? "coils" : "inputs")} starting at address {StartAddress}.";

    public void ValidateResponse(IModbusMessage response)
    {
        ReadCoilsInputsResponse? typedResponse = (ReadCoilsInputsResponse)response;

        // best effort validation - the same response for a request for 1 vs 6 coils (same byte count) will pass validation.
        int expectedByteCount = (NumberOfPoints + 7) / 8;

        if (expectedByteCount != typedResponse.ByteCount)
        {
            string msg = $"Unexpected byte count. Expected {expectedByteCount}, received {typedResponse.ByteCount}.";
            throw new IOException(msg);
        }
    }

    protected override void InitializeUnique(byte[] frame)
    {
        StartAddress = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 2));
        NumberOfPoints = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 4));
    }
}
