using Modbus.Data;
using Xunit;

namespace Modbus.UnitTests.Data;

public class DiscreteCollectionFixture
{
    [Fact]
    public void ByteCount()
    {
        DiscreteCollection col = new(true, true, false, false, false, false, false, false, false);
        Assert.Equal(2, col.ByteCount);
    }

    [Fact]
    public void ByteCountEven()
    {
        DiscreteCollection col = new(true, true, false, false, false, false, false, false);
        Assert.Equal(1, col.ByteCount);
    }

    [Fact]
    public void NetworkBytes()
    {
        DiscreteCollection col = new(true, true);
        Assert.Equal(new byte[] { 3 }, col.NetworkBytes);
    }

    [Fact]
    public void CreateNewDiscreteCollectionInitialize()
    {
        DiscreteCollection col = new(true, true, true);
        Assert.Equal(3, col.Count);
        Assert.DoesNotContain(false, col);
    }

    [Fact]
    public void CreateNewDiscreteCollectionFromBoolParams()
    {
        DiscreteCollection col = new(true, false, true);
        Assert.Equal(3, col.Count);
    }

    [Fact]
    public void CreateNewDiscreteCollectionFromBytesParams()
    {
        DiscreteCollection col = new(1, 2, 3);
        Assert.Equal(24, col.Count);
        bool[] expected =
        [
            true, false, false, false, false, false, false, false,
            false, true, false, false, false, false, false, false,
            true, true, false, false, false, false, false, false,
        ];

        Assert.Equal(expected, col);
    }

    [Fact]
    public void CreateNewDiscreteCollectionFromBytesParams_ZeroLengthArray()
    {
        DiscreteCollection col = new(Array.Empty<byte>());
        Assert.Empty(col);
    }

    [Fact]
    public void CreateNewDiscreteCollectionFromBytesParams_NullArray() =>
        Assert.Throws<ArgumentNullException>(() => new DiscreteCollection((byte[])null));

    [Fact]
    public void CreateNewDiscreteCollectionFromBytesParamsOrder()
    {
        DiscreteCollection col = new(194);
        Assert.Equal([false, true, false, false, false, false, true, true], col.ToArray());
    }

    [Fact]
    public void CreateNewDiscreteCollectionFromBytesParamsOrder2()
    {
        DiscreteCollection col = new(157, 7);
        Assert.Equal(
            [true, false, true, true, true, false, false, true, true, true, true, false, false, false, false, false],
            col.ToArray());
    }

    [Fact]
    public void Resize()
    {
        DiscreteCollection col = new(byte.MaxValue, byte.MaxValue);
        Assert.Equal(16, col.Count);
        col.RemoveAt(3);
        Assert.Equal(15, col.Count);
    }

    [Fact]
    public void BytesPersistence()
    {
        DiscreteCollection col = new(byte.MaxValue, byte.MaxValue);
        Assert.Equal(16, col.Count);
        byte[] originalBytes = col.NetworkBytes;
        col.RemoveAt(3);
        Assert.Equal(15, col.Count);
        Assert.NotEqual(originalBytes, col.NetworkBytes);
    }

    [Fact]
    public void AddCoil()
    {
        DiscreteCollection col = new();
        Assert.Empty(col);

        col.Add(true);
        Assert.Single(col);
    }
}