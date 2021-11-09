using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Modbus.Data;
using Modbus.Device;

namespace Modbus.IntegrationTests
{
    internal class TestCases
    {
        public static void Serial()
        {
            using (SerialPort? masterPort = new SerialPort("COM2"))
            using (SerialPort? slavePort = new SerialPort("COM1"))
            {
                // configure serial ports
                masterPort.BaudRate = slavePort.BaudRate = 9600;
                masterPort.DataBits = slavePort.DataBits = 8;
                masterPort.Parity = slavePort.Parity = Parity.None;
                masterPort.StopBits = slavePort.StopBits = StopBits.One;
                masterPort.Open();
                slavePort.Open();

                using (ModbusSerialSlave? slave = ModbusSerialSlave.CreateRtu(1, slavePort))
                {
                    StartSlave(slave);

                    // create modbus master
                    using (ModbusSerialMaster? master = ModbusSerialMaster.CreateRtu(masterPort))
                    {
                        ReadRegisters(master);
                    }
                }
            }
        }

        public static void Tcp()
        {
            TcpListener? slaveClient = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), 502);
            using (ModbusTcpSlave? slave = ModbusTcpSlave.CreateTcp((byte)1, slaveClient))
            {
                StartSlave(slave);

                IPAddress address = new(new byte[] { 127, 0, 0, 1 });
                TcpClient? masterClient = new TcpClient(address.ToString(), 502);

                using (ModbusIpMaster? master = ModbusIpMaster.CreateIp(masterClient))
                {
                    ReadRegisters(master);
                }
            }
        }

        public static void Udp()
        {
            UdpClient? slaveClient = new UdpClient(502);
            using (ModbusUdpSlave? slave = ModbusUdpSlave.CreateUdp(slaveClient))
            {
                StartSlave(slave);

                UdpClient? masterClient = new UdpClient();
                IPEndPoint endPoint = new(new IPAddress(new byte[] { 127, 0, 0, 1 }), 502);
                masterClient.Connect(endPoint);

                using (ModbusIpMaster? master = ModbusIpMaster.CreateIp(masterClient))
                {
                    ReadRegisters(master);
                }
            }
        }

        public static void StartSlave(ModbusSlave slave)
        {
            slave.DataStore = DataStoreFactory.CreateTestDataStore();
            Thread? slaveThread = new Thread(async () => await slave.ListenAsync());
            slaveThread.Start();
        }

        public static void ReadRegisters(IModbusMaster master)
        {
            ushort[]? result = master.ReadHoldingRegisters(1, 0, 5);

            for (int i = 0; i < 5; i++)
            {
                if (result[i] != i + 1)
                {
                    throw new Exception();
                }
            }
        }
    }
}
