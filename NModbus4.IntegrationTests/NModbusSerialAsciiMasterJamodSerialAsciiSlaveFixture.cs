using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public class NModbusSerialAsciiMasterJamodSerialAsciiSlaveFixture : ModbusMasterFixture
{
    private const string program = $"SerialSlave {DefaultSlaveSerialPortName} ASCII";

    public NModbusSerialAsciiMasterJamodSerialAsciiSlaveFixture()
    {
        StartJamodSlave(program);

        MasterSerialPort = CreateAndOpenSerialPort(DefaultMasterSerialPortName);
        Master = ModbusSerialMaster.CreateAscii(MasterSerialPort);
    }

    /// <summary>
    /// Jamod slave does not support this function
    /// </summary>
    public override void ReadWriteMultipleRegisters()
    {
    }

    [Fact]
    public override void ReadCoils() =>
        base.ReadCoils();
}
