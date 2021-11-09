using System.IO.Ports;
using System.Threading;
using Modbus.Data;
using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public class NModbusSerialRtuSlaveFixture
{
    [Fact]
    public void NModbusSerialRtuSlave_BonusCharacter_VerifyTimeout()
    {
        SerialPort masterPort = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName);
        SerialPort slavePort = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultSlaveSerialPortName);

        using ModbusSerialMaster? master = ModbusSerialMaster.CreateRtu(masterPort);
        using ModbusSerialSlave? slave = ModbusSerialSlave.CreateRtu(1, slavePort);
        master.Transport.ReadTimeout = master.Transport.WriteTimeout = 1000;
        slave.DataStore = DataStoreFactory.CreateTestDataStore();

        Thread slaveThread = new(async () => await slave.ListenAsync());
        slaveThread.IsBackground = true;
        slaveThread.Start();

        // assert successful communication
        Assert.Equal(new bool[] { false, true }, master.ReadCoils(1, 1, 2));

        // write "bonus" character
        masterPort.Write("*");

        // assert successful communication
        Assert.Equal(new bool[] { false, true }, master.ReadCoils(1, 1, 2));
    }
}
