using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Zion.API.Parser;

public struct SourceReader
{
    public string Source { get; init; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public int Index { get; private set; }

    public SourceReader(string source)
    {
        Source = source;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void Advance(int count = 1)
    {
        //Debug.Assert(Source.Length > Index + count);
        Index += count;
        Row += count;
    }

    public char Current => Source[Index];
    public bool EOF => Index == Source.Length;

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Peek(char c, int offset = 0)
    {
        Debug.Assert(Source.Length > Index + offset);
        return Source[Index + offset] == c;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public char Peek(int offset = 0)
    {
        Debug.Assert(Source.Length > offset + Index);
        return Source[Index + offset];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Location Location(int length = 1)
    {
        return new Location(Row, Column, length, Index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public void AdvanceColumn(int count = 1)
    {
        Row = 0;
        Column += count;
    }

    // TODO: ASSERT
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string? PeekValue(int length = 1, int offset = 0)
    {
        return Source.Substring(Index + offset, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int PeekWhile(char ch)
    {
        var length = 0;
        do
        {
            length++;
        } while (Peek(length) != ch);

        return length;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public string Display(Location location)
    {
        return Source.Substring(location.SourceIndex, location.Length);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Peek(int offset, char c)
    {
        return Peek(c, offset);
    }
}