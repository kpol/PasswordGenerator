namespace KPasswordGenerator;

public sealed class CharSet
{
    public CharSet(int count, string chars)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        ArgumentException.ThrowIfNullOrEmpty(chars);

        Count = count;
        Chars = chars;
    }

    public int Count { get; }

    public string Chars { get; }
}