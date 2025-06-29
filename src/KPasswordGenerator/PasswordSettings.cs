namespace KPasswordGenerator;

public sealed class PasswordSettings
{
    internal readonly string _allChars;
    internal readonly int _minimumPasswordLength;

    public PasswordSettings(ICollection<CharSet> charSets)
    {
        ArgumentNullException.ThrowIfNull(charSets);

        _minimumPasswordLength = charSets.Sum(c => c.Count);
        CharSets = charSets;
        _allChars = string.Concat(charSets.Select(c => c.Chars));
        
    }

    public ICollection<CharSet> CharSets { get; }
}