#pragma warning disable CA5394 // Do not use insecure randomness
using System.Diagnostics;
using System.Net.Sockets;
using Modbus.Data;
using Modbus.Device;
using Xunit;

namespace Modbus.IntegrationTests;

public class NModbusUdpSlaveFixture
{
    [Fact]
    public void ModbusUdpSlave_EnsureTheSlaveShutsDownCleanly()
    {
        UdpClient client = new(ModbusMasterFixture.Port);
        using ModbusUdpSlave slave = ModbusUdpSlave.CreateUdp(1, client);
        AutoResetEvent handle = new(false);

        Thread backgroundThread = new(async (state) =>
        {
            handle.Set();
            await slave.ListenAsync();
        })
        {
            IsBackground = true
        };
        backgroundThread.Start();

        handle.WaitOne();
        Thread.Sleep(100);
    }

    [Fact]
    public Task ModbusUdpSlave_NotBound()
    {
        UdpClient client = new();
        ModbusSlave slave = ModbusUdpSlave.CreateUdp(1, client);
        return Assert.ThrowsAsync<InvalidOperationException>(async () => await slave.ListenAsync());
    }

    [Fact]
    public void ModbusUdpSlave_MultipleMasters()
    {
        bool master1Complete = false;
        bool master2Complete = false;
        UdpClient masterClient1 = new();
        masterClient1.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
        ModbusIpMaster master1 = ModbusIpMaster.CreateIp(masterClient1);

        UdpClient masterClient2 = new();
        masterClient2.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
        ModbusIpMaster master2 = ModbusIpMaster.CreateIp(masterClient2);

        UdpClient slaveClient = CreateAndStartUdpSlave(ModbusMasterFixture.Port, DataStoreFactory.CreateTestDataStore());

        Thread master1Thread = new(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(Random.Shared.Next(1000));
                Debug.WriteLine("Read from master 1");
                Assert.Equal(new ushort[] { 2, 3, 4, 5, 6 }, master1.ReadHoldingRegisters(1, 5));
            }
            master1Complete = true;
        });

        Thread master2Thread = new(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(Random.Shared.Next(1000));
                Debug.WriteLine("Read from master 2");
                Assert.Equal(new ushort[] { 3, 4, 5, 6, 7 }, master2.ReadHoldingRegisters(2, 5));
            }
            master2Complete = true;
        });

        master1Thread.Start();
        master2Thread.Start();

        while (!master1Complete || !master2Complete)
        {
            Thread.Sleep(200);
        }

        slaveClient.Close();
        masterClient1.Close();
        masterClient2.Close();
    }

    [Fact]
    public void ModbusUdpSlave_MultiThreaded()
    {
        DataStore dataStore = DataStoreFactory.CreateDefaultDataStore();
        dataStore.CoilDiscretes.Add(false);

        using UdpClient slave = CreateAndStartUdpSlave(502, dataStore);
        Thread workerThread1 = new(ReadThread);
        Thread workerThread2 = new(ReadThread);
        workerThread1.Start();
        workerThread2.Start();

        workerThread1.Join();
        workerThread2.Join();
    }

    [Fact(Skip = "TODO consider supporting this scenario")]
    public void ModbusUdpSlave_SingleMasterPollingMultipleSlaves()
    {
        DataStore slave1DataStore = new();
        slave1DataStore.CoilDiscretes.Add(true);

        DataStore slave2DataStore = new();
        slave2DataStore.CoilDiscretes.Add(false);

        using UdpClient slave1 = CreateAndStartUdpSlave(502, slave1DataStore);
        using UdpClient slave2 = CreateAndStartUdpSlave(503, slave2DataStore);
        using UdpClient masterClient = new();
        masterClient.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
        ModbusIpMaster master = ModbusIpMaster.CreateIp(masterClient);

        for (int i = 0; i < 5; i++)
        {
            // we would need to create an overload taking in a port argument
            Assert.True(master.ReadCoils(0, 1)[0]);
            Assert.False(master.ReadCoils(1, 1)[0]);
        }
    }

    private static void ReadThread(object state)
    {
        UdpClient masterClient = new();
        masterClient.Connect(ModbusMasterFixture.DefaultModbusIPEndPoint);
        using ModbusIpMaster master = ModbusIpMaster.CreateIp(masterClient);
        master.Transport.Retries = 0;

        for (int i = 0; i < 5; i++)
        {
            bool[] coils = master.ReadCoils(1, 1);
            Assert.Single(coils);
            Debug.WriteLine($"{Environment.CurrentManagedThreadId}: Reading coil value");
            Thread.Sleep(Random.Shared.Next(100));
        }
    }

    private UdpClient CreateAndStartUdpSlave(int port, DataStore dataStore)
    {
        UdpClient slaveClient = new(port);
        ModbusSlave slave = ModbusUdpSlave.CreateUdp(slaveClient);
        slave.DataStore = dataStore;
        Thread slaveThread = new(async () => await slave.ListenAsync());
        slaveThread.Start();

        return slaveClient;
    }
}