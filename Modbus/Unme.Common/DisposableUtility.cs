namespace Modbus.Unme.Common;

internal static class DisposableUtility
{
    public static void Dispose<T>(ref T item)
        where T : class, IDisposable
    {
        if (item is null)
            return;

        item.Dispose();
        item = default;
    }
}