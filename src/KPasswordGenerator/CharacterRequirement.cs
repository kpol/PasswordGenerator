namespace KPasswordGenerator;

public sealed class CharacterRequirement
{
    public CharacterRequirement(int minRequired, string characterPool)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(minRequired);
        ArgumentException.ThrowIfNullOrEmpty(characterPool);

        MinRequired = minRequired;
        CharacterPool = characterPool;
    }

    public int MinRequired { get; }

    public string CharacterPool { get; }
}