using System.Text;

namespace Modbus.IO;

internal static class StreamResourceUtility
{
    internal static string ReadLine(IStreamResource stream)
    {
        StringBuilder? result = new();
        byte[]? singleByteBuffer = new byte[1];

        do
        {
            if (stream.Read(singleByteBuffer, 0, 1) == 0)
            {
                continue;
            }

            result.Append(Encoding.UTF8.GetChars(singleByteBuffer).First());
        }
        while (!result.ToString().EndsWith(Modbus.NewLine));

        return result.ToString().Substring(0, result.Length - Modbus.NewLine.Length);
    }
}
