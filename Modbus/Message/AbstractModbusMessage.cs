namespace Modbus.Message;

/// <summary>
///     Abstract Modbus message.
/// </summary>
public abstract class AbstractModbusMessage
{
    /// <summary>
    ///     Abstract Modbus message.
    /// </summary>
    private protected AbstractModbusMessage()
    {
        MessageImpl = new ModbusMessageImpl();
    }

    /// <summary>
    ///     Abstract Modbus message.
    /// </summary>
    private protected AbstractModbusMessage(byte slaveAddress, byte functionCode)
    {
        MessageImpl = new ModbusMessageImpl(slaveAddress, functionCode);
    }

    public ushort TransactionId
    {
        get => MessageImpl.TransactionId;
        set => MessageImpl.TransactionId = value;
    }

    public byte FunctionCode
    {
        get => MessageImpl.FunctionCode;
        set => MessageImpl.FunctionCode = value;
    }

    public byte SlaveAddress
    {
        get => MessageImpl.SlaveAddress;
        set => MessageImpl.SlaveAddress = value;
    }

    public virtual byte[] MessageFrame =>
        MessageImpl.MessageFrame;

    public virtual byte[] ProtocolDataUnit =>
        MessageImpl.ProtocolDataUnit;

    public abstract int MinimumFrameSize { get; }

    internal ModbusMessageImpl MessageImpl { get; }

    public void Initialize(byte[] frame)
    {
        if (frame.Length < MinimumFrameSize)
        {
            string msg = $"Message frame must contain at least {MinimumFrameSize} bytes of data.";
            throw new FormatException(msg);
        }

        MessageImpl.Initialize(frame);
        InitializeUnique(frame);
    }

    protected abstract void InitializeUnique(byte[] frame);
}
