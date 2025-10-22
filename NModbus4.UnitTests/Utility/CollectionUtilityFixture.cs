using System.Collections.ObjectModel;
using Modbus.Data;
using Modbus.UnitTests.Message;
using Modbus.Unme.Common;
using Xunit;

namespace Modbus.UnitTests.Utility;

public class CollectionUtilityFixture
{
    [Fact]
    public void SliceMiddle()
    {
        byte[] test = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Assert.Equal(new byte[] { 3, 4, 5, 6, 7 }, test.Slice(2, 5).ToArray());
    }

    [Fact]
    public void SliceBeginning()
    {
        byte[] test = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Assert.Equal(new byte[] { 1, 2 }, test.Slice(0, 2).ToArray());
    }

    [Fact]
    public void SliceEnd()
    {
        byte[] test = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Assert.Equal("\t\n"u8.ToArray(), test.Slice(8, 2).ToArray());
    }

    [Fact]
    public void SliceCollection()
    {
        Collection<bool> col = new([true, false, false, false, true, true]);
        Assert.Equal([false, false, true], col.Slice(2, 3).ToArray());
    }

    [Fact]
    public void SliceReadOnlyCollection()
    {
        ReadOnlyCollection<bool> col =
            new([true, false, false, false, true, true]);
        Assert.Equal([false, false, true], col.Slice(2, 3).ToArray());
    }

    [Fact]
    public void SliceNullICollection()
    {
        ICollection<bool> col = null;
        Assert.Throws<ArgumentNullException>(() => col.Slice(1, 1).ToArray());
    }

    [Fact]
    public void SliceNullArray()
    {
        bool[] array = null;
        Assert.Throws<ArgumentNullException>(() => array.Slice(1, 1).ToArray());
    }

    [Fact]
    public void CreateDefaultCollectionNegativeSize() =>
        Assert.Throws<ArgumentOutOfRangeException>(() => MessageUtility.CreateDefaultCollection<RegisterCollection, ushort>(0, -1));

    [Fact]
    public void CreateDefaultCollection()
    {
        RegisterCollection col = MessageUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, 5);
        Assert.Equal(5, col.Count);
        Assert.Equal(new ushort[] { 3, 3, 3, 3, 3 }, col.ToArray());
    }
}