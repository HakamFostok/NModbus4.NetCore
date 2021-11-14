using Modbus.Data;
using Modbus.Unme.Common;

namespace Modbus.Message;

public class ReadHoldingInputRegistersResponse : AbstractModbusMessageWithData<RegisterCollection>, IModbusMessage
{
    public ReadHoldingInputRegistersResponse()
    {
    }

    public ReadHoldingInputRegistersResponse(byte functionCode, byte slaveAddress, RegisterCollection data)
        : base(slaveAddress, functionCode)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        ByteCount = data.ByteCount;
        Data = data;
    }

    public byte ByteCount
    {
        get => MessageImpl.ByteCount.Value;
        set => MessageImpl.ByteCount = value;
    }

    public override int MinimumFrameSize => 3;

    public override string ToString() =>
        $"Read {Data.Count} {(FunctionCode == Modbus.ReadHoldingRegisters ? "holding" : "input")} registers.";

    protected override void InitializeUnique(byte[] frame)
    {
        if (frame.Length < MinimumFrameSize + frame[2])
        {
            throw new FormatException("Message frame does not contain enough bytes.");
        }

        ByteCount = frame[2];
        Data = new RegisterCollection(frame.Slice(3, ByteCount).ToArray());
    }
}