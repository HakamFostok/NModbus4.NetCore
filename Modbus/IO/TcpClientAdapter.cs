using System.Diagnostics;
using System.Net.Sockets;
using Modbus.Unme.Common;

namespace Modbus.IO;

/// <summary>
///     Concrete Implementor - http://en.wikipedia.org/wiki/Bridge_Pattern
/// </summary>
internal class TcpClientAdapter : IStreamResource
{
    private TcpClient _tcpClient;

    public TcpClientAdapter(TcpClient tcpClient)
    {
        Debug.Assert(tcpClient != null, "Argument tcpClient cannot be null.");

        _tcpClient = tcpClient;
    }

    public int InfiniteTimeout => Timeout.Infinite;

    public int ReadTimeout
    {
        get => _tcpClient.GetStream().ReadTimeout;
        set => _tcpClient.GetStream().ReadTimeout = value;
    }

    public int WriteTimeout
    {
        get => _tcpClient.GetStream().WriteTimeout;
        set => _tcpClient.GetStream().WriteTimeout = value;
    }

    public void Write(byte[] buffer, int offset, int count) =>
        _tcpClient.GetStream().Write(buffer, offset, count);

    public int Read(byte[] buffer, int offset, int count) =>
        _tcpClient.GetStream().Read(buffer, offset, count);

    public void DiscardInBuffer() =>
        _tcpClient.GetStream().Flush();

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposableUtility.Dispose(ref _tcpClient);
        }
    }
}