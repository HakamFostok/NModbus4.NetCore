﻿using System.IO.Ports;
using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public class NModbusSerialRtuMasterFixture
{
    [Fact]
    public void NModbusRtuMaster_ReadTimeout()
    {
        SerialPort port = ModbusMasterFixture.CreateAndOpenSerialPort(ModbusMasterFixture.DefaultMasterSerialPortName);
        using ModbusSerialMaster? master = ModbusSerialMaster.CreateRtu(port);
        master.Transport.ReadTimeout = master.Transport.WriteTimeout = 1000;
        master.ReadCoils(100, 1, 1);
    }
}
