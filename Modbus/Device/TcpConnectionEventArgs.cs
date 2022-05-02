namespace Modbus.Device;

internal class TcpConnectionEventArgs : EventArgs
{
    public TcpConnectionEventArgs(string endPoint)
    {
        ArgumentNullException.ThrowIfNull(endPoint);

        if (endPoint == string.Empty)
        {
            throw new ArgumentException(Resources.EmptyEndPoint);
        }

        EndPoint = endPoint;
    }

    public string EndPoint { get; set; }
}