using System.Net.Sockets;
using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public class ModbusIpMasterFixture
{
    [Fact]
    public void OverrideTimeoutOnTcpClient()
    {
        TcpListener listener = new(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
        using ModbusTcpSlave slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, listener);
        Thread slaveThread = new(async () => await slave.ListenAsync());
        slaveThread.Start();

        TcpClient client = new(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port)
        {
            ReceiveTimeout = 1500,
            SendTimeout = 3000
        };
        using ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
        Assert.Equal(1500, client.GetStream().ReadTimeout);
        Assert.Equal(3000, client.GetStream().WriteTimeout);
    }

    [Fact]
    public void OverrideTimeoutOnNetworkStream()
    {
        TcpListener listener = new(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
        using ModbusTcpSlave slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, listener);
        Thread slaveThread = new(async () => await slave.ListenAsync());
        slaveThread.Start();

        TcpClient client = new(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port);
        client.GetStream().ReadTimeout = 1500;
        client.GetStream().WriteTimeout = 3000;
        using ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
        Assert.Equal(1500, client.GetStream().ReadTimeout);
        Assert.Equal(3000, client.GetStream().WriteTimeout);
    }
}