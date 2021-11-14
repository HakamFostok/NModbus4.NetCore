﻿using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public abstract class ModbusSerialMasterFixture : ModbusMasterFixture
{
    [Fact]
    public virtual void ReturnQueryData()
    {
        Assert.True(((ModbusSerialMaster)Master).ReturnQueryData(SlaveAddress, 18));
        Assert.True(((ModbusSerialMaster)Master).ReturnQueryData(SlaveAddress, 5));
    }
}