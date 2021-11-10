﻿using System.Diagnostics;
using System.IO;

using Modbus.Message;

namespace Modbus.IO;

/// <summary>
///     Transport for Serial protocols.
///     Refined Abstraction - http://en.wikipedia.org/wiki/Bridge_Pattern
/// </summary>
public abstract class ModbusSerialTransport : ModbusTransport
{
    internal ModbusSerialTransport(IStreamResource streamResource)
        : base(streamResource)
    {
        Debug.Assert(streamResource != null, "Argument streamResource cannot be null.");
    }

    /// <summary>
    ///     Gets or sets a value indicating whether LRC/CRC frame checking is performed on messages.
    /// </summary>
    public bool CheckFrame { get; set; } = true;

    internal void DiscardInBuffer() =>
        StreamResource.DiscardInBuffer();

    internal override void Write(IModbusMessage message)
    {
        DiscardInBuffer();

        byte[] frame = BuildMessageFrame(message);
        Debug.WriteLine($"TX: {string.Join(", ", frame)}");
        StreamResource.Write(frame, 0, frame.Length);
    }

    internal override IModbusMessage CreateResponse<T>(byte[] frame)
    {
        IModbusMessage response = base.CreateResponse<T>(frame);

        // compare checksum
        if (CheckFrame && !ChecksumsMatch(response, frame))
        {
            string msg = $"Checksums failed to match {string.Join(", ", response.MessageFrame)} != {string.Join(", ", frame)}";
            Debug.WriteLine(msg);
            throw new IOException(msg);
        }

        return response;
    }

    internal abstract bool ChecksumsMatch(IModbusMessage message, byte[] messageFrame);

    internal override void OnValidateResponse(IModbusMessage request, IModbusMessage response)
    {
        // no-op
    }
}