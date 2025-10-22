using Modbus.Serial;
using System.Net.Sockets;
using Modbus.Data;
using Modbus.IO;
using Modbus.Message;

#if SERIAL
using System.IO.Ports;
#endif

namespace Modbus.Device;

/// <summary>
///     Modbus serial master device.
/// </summary>
public class ModbusSerialMaster : ModbusMaster, IModbusSerialMaster
{
    private ModbusSerialMaster(ModbusTransport transport)
        : base(transport)
    {
    }

    /// <summary>
    ///     Gets the Modbus Transport.
    /// </summary>
    ModbusSerialTransport IModbusSerialMaster.Transport =>
        (ModbusSerialTransport)Transport;

#if SERIAL
    /// <summary>
    ///     Modbus ASCII master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateAscii(SerialPort serialPort)
    {
        ArgumentNullException.ThrowIfNull(serialPort);

        return CreateAscii(new SerialPortAdapter(serialPort));
    }
#endif

    /// <summary>
    ///     Modbus ASCII master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateAscii(TcpClient tcpClient)
    {
        ArgumentNullException.ThrowIfNull(tcpClient);

        return CreateAscii(new TcpClientAdapter(tcpClient));
    }

    /// <summary>
    ///     Modbus ASCII master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateAscii(UdpClient udpClient)
    {
        ArgumentNullException.ThrowIfNull(udpClient);

        if (!udpClient.Client.Connected)
        {
            throw new InvalidOperationException(Resources.UdpClientNotConnected);
        }

        return CreateAscii(new UdpClientAdapter(udpClient));
    }

    /// <summary>
    ///     Modbus ASCII master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateAscii(IStreamResource streamResource)
    {
        ArgumentNullException.ThrowIfNull(streamResource);

        return new ModbusSerialMaster(new ModbusAsciiTransport(streamResource));
    }

#if SERIAL
    /// <summary>
    ///     Modbus RTU master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateRtu(SerialPort serialPort)
    {
        ArgumentNullException.ThrowIfNull(serialPort);

        return CreateRtu(new SerialPortAdapter(serialPort));
    }
#endif

    /// <summary>
    ///     Modbus RTU master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateRtu(TcpClient tcpClient)
    {
        ArgumentNullException.ThrowIfNull(tcpClient);

        return CreateRtu(new TcpClientAdapter(tcpClient));
    }

    /// <summary>
    ///     Modbus RTU master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateRtu(UdpClient udpClient)
    {
        ArgumentNullException.ThrowIfNull(udpClient);

        if (!udpClient.Client.Connected)
        {
            throw new InvalidOperationException(Resources.UdpClientNotConnected);
        }

        return CreateRtu(new UdpClientAdapter(udpClient));
    }

    /// <summary>
    ///     Modbus RTU master factory method.
    /// </summary>
    public static ModbusSerialMaster CreateRtu(IStreamResource streamResource)
    {
        ArgumentNullException.ThrowIfNull(streamResource);

        return new ModbusSerialMaster(new ModbusRtuTransport(streamResource));
    }

    /// <summary>
    ///     Serial Line only.
    ///     Diagnostic function which loops back the original data.
    ///     NModbus only supports looping back one ushort value, this is a limitation of the "Best Effort" implementation of
    ///     the RTU protocol.
    /// </summary>
    /// <param name="slaveAddress">Address of device to test.</param>
    /// <param name="data">Data to return.</param>
    /// <returns>Return true if slave device echoed data.</returns>
    public bool ReturnQueryData(byte slaveAddress, ushort data)
    {
        DiagnosticsRequestResponse request = new(
            Modbus.DiagnosticsReturnQueryData,
            slaveAddress,
            new RegisterCollection(data));

        DiagnosticsRequestResponse response = Transport.UnicastMessage<DiagnosticsRequestResponse>(request);

        return response.Data[0] == data;
    }
}