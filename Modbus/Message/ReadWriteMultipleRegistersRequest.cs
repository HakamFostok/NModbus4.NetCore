﻿using System;
using System.IO;

using Modbus.Data;

namespace Modbus.Message;

public class ReadWriteMultipleRegistersRequest : AbstractModbusMessage, IModbusRequest
{
    private ReadHoldingInputRegistersRequest _readRequest;
    private WriteMultipleRegistersRequest _writeRequest;

    public ReadWriteMultipleRegistersRequest()
    {
    }

    public ReadWriteMultipleRegistersRequest(
        byte slaveAddress,
        ushort startReadAddress,
        ushort numberOfPointsToRead,
        ushort startWriteAddress,
        RegisterCollection writeData)
        : base(slaveAddress, Modbus.ReadWriteMultipleRegisters)
    {
        _readRequest = new ReadHoldingInputRegistersRequest(
            Modbus.ReadHoldingRegisters,
            slaveAddress,
            startReadAddress,
            numberOfPointsToRead);

        _writeRequest = new WriteMultipleRegistersRequest(
            slaveAddress,
            startWriteAddress,
            writeData);
    }

    public override byte[] ProtocolDataUnit
    {
        get
        {
            byte[] readPdu = _readRequest.ProtocolDataUnit;
            byte[] writePdu = _writeRequest.ProtocolDataUnit;
            MemoryStream? stream = new(readPdu.Length + writePdu.Length);

            stream.WriteByte(FunctionCode);

            // read and write PDUs without function codes
            stream.Write(readPdu, 1, readPdu.Length - 1);
            stream.Write(writePdu, 1, writePdu.Length - 1);

            return stream.ToArray();
        }
    }

    public ReadHoldingInputRegistersRequest ReadRequest => _readRequest;

    public WriteMultipleRegistersRequest WriteRequest => _writeRequest;

    public override int MinimumFrameSize => 11;

    public override string ToString() =>
        $"Write {_writeRequest.NumberOfPoints} holding registers starting at address {_writeRequest.StartAddress}, and read {_readRequest.NumberOfPoints} registers starting at address {_readRequest.StartAddress}.";

    public void ValidateResponse(IModbusMessage response)
    {
        ReadHoldingInputRegistersResponse? typedResponse = (ReadHoldingInputRegistersResponse)response;
        int expectedByteCount = ReadRequest.NumberOfPoints * 2;

        if (expectedByteCount != typedResponse.ByteCount)
        {
            string msg = $"Unexpected byte count in response. Expected {expectedByteCount}, received {typedResponse.ByteCount}.";
            throw new IOException(msg);
        }
    }

    protected override void InitializeUnique(byte[] frame)
    {
        if (frame.Length < MinimumFrameSize + frame[10])
        {
            throw new FormatException("Message frame does not contain enough bytes.");
        }

        byte[] readFrame = new byte[2 + 4];
        byte[] writeFrame = new byte[frame.Length - 6 + 2];

        readFrame[0] = writeFrame[0] = SlaveAddress;
        readFrame[1] = writeFrame[1] = FunctionCode;

        Buffer.BlockCopy(frame, 2, readFrame, 2, 4);
        Buffer.BlockCopy(frame, 6, writeFrame, 2, frame.Length - 6);

        _readRequest = ModbusMessageFactory.CreateModbusMessage<ReadHoldingInputRegistersRequest>(readFrame);
        _writeRequest = ModbusMessageFactory.CreateModbusMessage<WriteMultipleRegistersRequest>(writeFrame);
    }
}
