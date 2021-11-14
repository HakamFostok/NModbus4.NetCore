namespace Modbus.Unme.Common;

internal static class SequenceUtility
{
    public static IEnumerable<T> Slice<T>(this IEnumerable<T> source, int startIndex, int size)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        T[]? enumerable = source as T[] ?? source.ToArray();
        int num = enumerable.Count();

        if (startIndex < 0 || num < startIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }

        if (size < 0 || startIndex + size > num)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        return enumerable.Skip(startIndex).Take(size);
    }
}