﻿using Modbus.Data;
using Modbus.IO;
using Modbus.Message;

namespace Modbus.Device;

/// <summary>
///     Modbus master device.
/// </summary>
public abstract class ModbusMaster : ModbusDevice, IModbusMaster
{
    private protected ModbusMaster(ModbusTransport transport)
        : base(transport)
    {
    }

    /// <summary>
    ///    Reads from 1 to 2000 contiguous coils status.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of coils to read.</param>
    /// <returns>Coils status.</returns>
    public bool[] ReadCoils(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 2000);

        ReadCoilsInputsRequest request = new(
            Modbus.ReadCoils,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadDiscretes(request);
    }

    /// <summary>
    ///    Asynchronously reads from 1 to 2000 contiguous coils status.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of coils to read.</param>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public Task<bool[]> ReadCoilsAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 2000);

        ReadCoilsInputsRequest request = new(
            Modbus.ReadCoils,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadDiscretesAsync(request);
    }

    /// <summary>
    ///    Reads from 1 to 2000 contiguous discrete input status.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of discrete inputs to read.</param>
    /// <returns>Discrete inputs status.</returns>
    public bool[] ReadInputs(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 2000);

        ReadCoilsInputsRequest request = new(
            Modbus.ReadInputs,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadDiscretes(request);
    }

    /// <summary>
    ///    Asynchronously reads from 1 to 2000 contiguous discrete input status.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of discrete inputs to read.</param>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public Task<bool[]> ReadInputsAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 2000);

        ReadCoilsInputsRequest request = new(
            Modbus.ReadInputs,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadDiscretesAsync(request);
    }

    /// <summary>
    ///    Reads contiguous block of holding registers.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of holding registers to read.</param>
    /// <returns>Holding registers status.</returns>
    public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 125);

        ReadHoldingInputRegistersRequest request = new(
            Modbus.ReadHoldingRegisters,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadRegisters(request);
    }

    /// <summary>
    ///    Asynchronously reads contiguous block of holding registers.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of holding registers to read.</param>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public Task<ushort[]> ReadHoldingRegistersAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 125);

        ReadHoldingInputRegistersRequest request = new(
            Modbus.ReadHoldingRegisters,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadRegistersAsync(request);
    }

    /// <summary>
    ///    Reads contiguous block of input registers.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of holding registers to read.</param>
    /// <returns>Input registers status.</returns>
    public ushort[] ReadInputRegisters(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 125);

        ReadHoldingInputRegistersRequest request = new(
            Modbus.ReadInputRegisters,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadRegisters(request);
    }

    /// <summary>
    ///    Asynchronously reads contiguous block of input registers.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startAddress">Address to begin reading.</param>
    /// <param name="numberOfPoints">Number of holding registers to read.</param>
    /// <returns>A task that represents the asynchronous read operation.</returns>
    public Task<ushort[]> ReadInputRegistersAsync(byte slaveAddress, ushort startAddress, ushort numberOfPoints)
    {
        ValidateNumberOfPoints(nameof(numberOfPoints), numberOfPoints, 125);

        ReadHoldingInputRegistersRequest request = new(
            Modbus.ReadInputRegisters,
            slaveAddress,
            startAddress,
            numberOfPoints);

        return PerformReadRegistersAsync(request);
    }

    /// <summary>
    ///    Writes a single coil value.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="coilAddress">Address to write value to.</param>
    /// <param name="value">Value to write.</param>
    public void WriteSingleCoil(byte slaveAddress, ushort coilAddress, bool value)
    {
        WriteSingleCoilRequestResponse request = new(slaveAddress, coilAddress, value);
        Transport.UnicastMessage<WriteSingleCoilRequestResponse>(request);
    }

    /// <summary>
    ///    Asynchronously writes a single coil value.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="coilAddress">Address to write value to.</param>
    /// <param name="value">Value to write.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task WriteSingleCoilAsync(byte slaveAddress, ushort coilAddress, bool value)
    {
        WriteSingleCoilRequestResponse request = new(slaveAddress, coilAddress, value);
        return PerformWriteRequestAsync<WriteSingleCoilRequestResponse>(request);
    }

    /// <summary>
    ///    Writes a single holding register.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="registerAddress">Address to write.</param>
    /// <param name="value">Value to write.</param>
    public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort value)
    {
        WriteSingleRegisterRequestResponse request = new(
            slaveAddress,
            registerAddress,
            value);

        Transport.UnicastMessage<WriteSingleRegisterRequestResponse>(request);
    }

    /// <summary>
    ///    Asynchronously writes a single holding register.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="registerAddress">Address to write.</param>
    /// <param name="value">Value to write.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task WriteSingleRegisterAsync(byte slaveAddress, ushort registerAddress, ushort value)
    {
        WriteSingleRegisterRequestResponse request = new(
            slaveAddress,
            registerAddress,
            value);

        return PerformWriteRequestAsync<WriteSingleRegisterRequestResponse>(request);
    }

    /// <summary>
    ///     Write a block of 1 to 123 contiguous 16 bit holding registers.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="startAddress">Address to begin writing values.</param>
    /// <param name="data">Values to write.</param>
    public void WriteMultipleRegisters(byte slaveAddress, ushort startAddress, ushort[] data)
    {
        ValidateData(nameof(data), data, 123);

        WriteMultipleRegistersRequest request = new(
            slaveAddress,
            startAddress,
            new RegisterCollection(data));

        Transport.UnicastMessage<WriteMultipleRegistersResponse>(request);
    }

    /// <summary>
    ///    Asynchronously writes a block of 1 to 123 contiguous registers.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="startAddress">Address to begin writing values.</param>
    /// <param name="data">Values to write.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task WriteMultipleRegistersAsync(byte slaveAddress, ushort startAddress, ushort[] data)
    {
        ValidateData(nameof(data), data, 123);

        WriteMultipleRegistersRequest request = new(
            slaveAddress,
            startAddress,
            new RegisterCollection(data));

        return PerformWriteRequestAsync<WriteMultipleRegistersResponse>(request);
    }

    /// <summary>
    ///    Writes a sequence of coils.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="startAddress">Address to begin writing values.</param>
    /// <param name="data">Values to write.</param>
    public void WriteMultipleCoils(byte slaveAddress, ushort startAddress, bool[] data)
    {
        ValidateData(nameof(data), data, 1968);

        WriteMultipleCoilsRequest request = new(
            slaveAddress,
            startAddress,
            new DiscreteCollection(data));

        Transport.UnicastMessage<WriteMultipleCoilsResponse>(request);
    }

    /// <summary>
    ///    Asynchronously writes a sequence of coils.
    /// </summary>
    /// <param name="slaveAddress">Address of the device to write to.</param>
    /// <param name="startAddress">Address to begin writing values.</param>
    /// <param name="data">Values to write.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    public Task WriteMultipleCoilsAsync(byte slaveAddress, ushort startAddress, bool[] data)
    {
        ValidateData(nameof(data), data, 1968);

        WriteMultipleCoilsRequest request = new(
            slaveAddress,
            startAddress,
            new DiscreteCollection(data));

        return PerformWriteRequestAsync<WriteMultipleCoilsResponse>(request);
    }

    /// <summary>
    ///    Performs a combination of one read operation and one write operation in a single Modbus transaction.
    ///    The write operation is performed before the read.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
    /// <param name="numberOfPointsToRead">Number of registers to read.</param>
    /// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
    /// <param name="writeData">Register values to write.</param>
    public ushort[] ReadWriteMultipleRegisters(
        byte slaveAddress,
        ushort startReadAddress,
        ushort numberOfPointsToRead,
        ushort startWriteAddress,
        ushort[] writeData)
    {
        ValidateNumberOfPoints("numberOfPointsToRead", numberOfPointsToRead, 125);
        ValidateData(nameof(writeData), writeData, 121);

        ReadWriteMultipleRegistersRequest request = new(slaveAddress, startReadAddress, numberOfPointsToRead, startWriteAddress, new RegisterCollection(writeData));

        return PerformReadRegisters(request);
    }

    /// <summary>
    ///    Asynchronously performs a combination of one read operation and one write operation in a single Modbus transaction.
    ///    The write operation is performed before the read.
    /// </summary>
    /// <param name="slaveAddress">Address of device to read values from.</param>
    /// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
    /// <param name="numberOfPointsToRead">Number of registers to read.</param>
    /// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
    /// <param name="writeData">Register values to write.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task<ushort[]> ReadWriteMultipleRegistersAsync(
        byte slaveAddress,
        ushort startReadAddress,
        ushort numberOfPointsToRead,
        ushort startWriteAddress,
        ushort[] writeData)
    {
        ValidateNumberOfPoints("numberOfPointsToRead", numberOfPointsToRead, 125);
        ValidateData(nameof(writeData), writeData, 121);

        ReadWriteMultipleRegistersRequest request = new(slaveAddress, startReadAddress, numberOfPointsToRead, startWriteAddress, new RegisterCollection(writeData));

        return PerformReadRegistersAsync(request);
    }

    /// <summary>
    ///    Executes the custom message.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <param name="request">The request.</param>
    public TResponse? ExecuteCustomMessage<TResponse>(IModbusMessage request)
        where TResponse : IModbusMessage, new() =>
        Transport.UnicastMessage<TResponse>(request);

    private static void ValidateData<T>(string argumentName, T[] data, int maxDataLength)
    {
        ArgumentNullException.ThrowIfNull(data);

        if (data.Length == 0 || data.Length > maxDataLength)
        {
            string msg = $"The length of argument {argumentName} must be between 1 and {maxDataLength} inclusive.";
            throw new ArgumentException(msg);
        }
    }

    private static void ValidateNumberOfPoints(string argumentName, ushort numberOfPoints, ushort maxNumberOfPoints)
    {
        if (numberOfPoints < 1 || numberOfPoints > maxNumberOfPoints)
        {
            string msg = $"Argument {argumentName} must be between 1 and {maxNumberOfPoints} inclusive.";
            throw new ArgumentException(msg);
        }
    }

    private bool[] PerformReadDiscretes(ReadCoilsInputsRequest request)
    {
        ReadCoilsInputsResponse? response = Transport.UnicastMessage<ReadCoilsInputsResponse>(request);
        return response.Data.Take(request.NumberOfPoints).ToArray();
    }

    private Task<bool[]> PerformReadDiscretesAsync(ReadCoilsInputsRequest request) =>
        Task.Factory.StartNew(() => PerformReadDiscretes(request));

    private ushort[] PerformReadRegisters(ReadHoldingInputRegistersRequest request)
    {
        ReadHoldingInputRegistersResponse response =
            Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);

        return response.Data.Take(request.NumberOfPoints).ToArray();
    }

    private ushort[] PerformReadRegisters(ReadWriteMultipleRegistersRequest request)
    {
        ReadHoldingInputRegistersResponse response =
            Transport.UnicastMessage<ReadHoldingInputRegistersResponse>(request);

        return response.Data.Take(request.ReadRequest.NumberOfPoints).ToArray();
    }

    private Task<ushort[]> PerformReadRegistersAsync(ReadHoldingInputRegistersRequest request) =>
        Task.Factory.StartNew(() => PerformReadRegisters(request));

    private Task<ushort[]> PerformReadRegistersAsync(ReadWriteMultipleRegistersRequest request) =>
        Task.Factory.StartNew(() => PerformReadRegisters(request));

    private Task PerformWriteRequestAsync<T>(IModbusMessage request)
        where T : IModbusMessage, new() =>
        Task.Factory.StartNew(() => Transport.UnicastMessage<T>(request));
}