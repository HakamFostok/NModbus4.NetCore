using Modbus.Message;

namespace Modbus.Device;

/// <summary>
///     Modbus Slave request event args containing information on the message.
/// </summary>
public class ModbusSlaveRequestEventArgs : EventArgs
{
    internal ModbusSlaveRequestEventArgs(IModbusMessage message)
    {
        Message = message;
    }

    /// <summary>
    ///     Gets the message.
    /// </summary>
    public IModbusMessage Message { get; }
}