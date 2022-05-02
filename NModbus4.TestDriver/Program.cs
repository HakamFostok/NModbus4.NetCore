using System;
using System.Net.Sockets;
using Modbus.Device;

try
{
    using TcpClient client = new("127.0.0.1", 502);
    client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

    ModbusIpMaster master = ModbusIpMaster.CreateIp(client);
    // read five input values
    ushort startAddress = 100;
    ushort numInputs = 5;
    bool[] inputs = master.ReadInputs(startAddress, numInputs);

    for (int i = 0; i < numInputs; i++)
    {
        Console.WriteLine($"Input {(startAddress + i)}={(inputs[i] ? 1 : 0)}");
    }

    while (true)
    {
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}