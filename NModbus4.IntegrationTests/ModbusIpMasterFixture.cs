using System.Net.Sockets;
using System.Threading;
using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests
{
    public class ModbusIpMasterFixture
    {
        [Fact]
        public void OverrideTimeoutOnTcpClient()
        {
            TcpListener? listener = new TcpListener(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
            using (ModbusTcpSlave? slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, listener))
            {
                Thread? slaveThread = new Thread(async () => await slave.ListenAsync());
                slaveThread.Start();

                TcpClient? client = new TcpClient(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port);
                client.ReceiveTimeout = 1500;
                client.SendTimeout = 3000;
                using (ModbusIpMaster? master = ModbusIpMaster.CreateIp(client))
                {
                    Assert.Equal(1500, client.GetStream().ReadTimeout);
                    Assert.Equal(3000, client.GetStream().WriteTimeout);
                }
            }
        }

        [Fact]
        public void OverrideTimeoutOnNetworkStream()
        {
            TcpListener? listener = new TcpListener(ModbusMasterFixture.TcpHost, ModbusMasterFixture.Port);
            using (ModbusTcpSlave? slave = ModbusTcpSlave.CreateTcp(ModbusMasterFixture.SlaveAddress, listener))
            {
                Thread? slaveThread = new Thread(async () => await slave.ListenAsync());
                slaveThread.Start();

                TcpClient? client = new TcpClient(ModbusMasterFixture.TcpHost.ToString(), ModbusMasterFixture.Port);
                client.GetStream().ReadTimeout = 1500;
                client.GetStream().WriteTimeout = 3000;
                using (ModbusIpMaster? master = ModbusIpMaster.CreateIp(client))
                {
                    Assert.Equal(1500, client.GetStream().ReadTimeout);
                    Assert.Equal(3000, client.GetStream().WriteTimeout);
                }
            }
        }
    }
}
