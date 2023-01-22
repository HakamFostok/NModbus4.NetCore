using System.Diagnostics.CodeAnalysis;

namespace Modbus.Data;

/// <summary>
///     Modbus message containing data.
/// </summary>
public interface IModbusMessageDataCollection
{
    /// <summary>
    ///     Gets the network bytes.
    /// </summary>
    byte[] NetworkBytes { get; }

    /// <summary>
    ///     Gets the byte count.
    /// </summary>
    byte ByteCount { get; }
}