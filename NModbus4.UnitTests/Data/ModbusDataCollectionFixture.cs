using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modbus.Data;
using Xunit;

namespace Modbus.UnitTests.Data;

public abstract class ModbusDataCollectionFixture<TData>
{
    [Fact]
    public void DefaultContstructor()
    {
        ModbusDataCollection<TData>? col = new();
        Assert.NotEmpty(col);
        Assert.Equal(1, col.Count);

        col.Add(default(TData));
        Assert.Equal(2, col.Count);
    }

    [Fact]
    public void ContstructorWithParams()
    {
        TData[] source = GetArray();
        ModbusDataCollection<TData>? col = new(source);
        Assert.Equal(source.Length + 1, col.Count);
        Assert.NotEmpty(col);

        col.Add(default(TData));
        Assert.Equal(source.Length + 2, col.Count);
    }

    [Fact]
    public void ContstructorWithIList()
    {
        List<TData> source = GetList();
        int expectedCount = source.Count;

        ModbusDataCollection<TData>? col = new(source);

        Assert.Equal(expectedCount + 1, source.Count);
        Assert.Equal(expectedCount + 1, col.Count);

        source.Insert(0, default(TData));
        Assert.Equal(source, col);
    }

    [Fact]
    public void ContstructorWithIList_FromReadOnlyList()
    {
        List<TData> source = GetList();
        ReadOnlyCollection<TData>? readOnly = new(source);
        int expectedCount = source.Count;

        ModbusDataCollection<TData>? col = new(readOnly);

        Assert.Equal(expectedCount, source.Count);
        Assert.Equal(expectedCount + 1, col.Count);

        source.Insert(0, default(TData));
        Assert.Equal(source, col);
    }

    [Fact]
    public void SetZeroElementUsingItem()
    {
        TData[]? source = GetArray();
        ModbusDataCollection<TData>? col = new(source);
        Assert.Throws<ArgumentOutOfRangeException>(() => col[0] = source[3]);
    }

    [Fact]
    public void ZeroElementUsingItem_Negative()
    {
        TData[]? source = GetArray();
        ModbusDataCollection<TData>? col = new(source);

        Assert.Throws<ArgumentOutOfRangeException>(() => col[0] = source[3]);
        Assert.Throws<ArgumentOutOfRangeException>(() => col.Insert(0, source[3]));
        Assert.Throws<ArgumentOutOfRangeException>(() => col.RemoveAt(0));

        // Remove forst zero/false
        Assert.Throws<ArgumentOutOfRangeException>(() => col.Remove(default(TData)));
    }

    [Fact]
    public void Clear()
    {
        ModbusDataCollection<TData>? col = new(GetArray());
        col.Clear();

        Assert.Equal(1, col.Count);
    }

    [Fact]
    public void Remove()
    {
        List<TData> source = GetList();
        ModbusDataCollection<TData>? col = new(source);
        int expectedCount = source.Count - 1;

        Assert.True(col.Remove(source[3]));

        Assert.Equal(expectedCount, col.Count);
        Assert.Equal(expectedCount, source.Count);
        Assert.Equal(source, col);
    }

    protected abstract TData[] GetArray();

    protected abstract TData GetNonExistentElement();

    protected List<TData> GetList() => 
        new(GetArray());
}
