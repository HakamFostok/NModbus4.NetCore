using Modbus.Data;

namespace Modbus.Message;

public abstract class AbstractModbusMessageWithData<TData> : AbstractModbusMessage
    where TData : IModbusMessageDataCollection
{
    private protected AbstractModbusMessageWithData()
    {
    }

    private protected AbstractModbusMessageWithData(byte slaveAddress, byte functionCode)
        : base(slaveAddress, functionCode)
    {
    }

    public TData? Data
    {
        get => (TData)MessageImpl.Data;
        set => MessageImpl.Data = value;
    }
}