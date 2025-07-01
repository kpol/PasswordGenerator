namespace KPasswordGenerator;

public sealed class PasswordSettings
{
    internal readonly string AllChars;
    internal readonly int MinimumPasswordLength;

    public PasswordSettings(ICollection<CharacterRequirement> characterRequirements)
    {
        ArgumentNullException.ThrowIfNull(characterRequirements);

        MinimumPasswordLength = characterRequirements.Sum(c => c.MinRequired);
        CharacterRequirements = characterRequirements;
        AllChars = string.Concat(characterRequirements.Select(c => c.CharacterPool));
        
    }

    public ICollection<CharacterRequirement> CharacterRequirements { get; }

    public static PasswordSettings WithDefaults(int minLower = 1, int minUpper = 1, int minDigits = 1, int minSpecial = 1)
    {
        List<CharacterRequirement> requirements = [];

        if (minLower > 0)
            requirements.Add(new CharacterRequirement(minLower, CharacterPools.LowerCase));

        if (minUpper > 0)
            requirements.Add(new CharacterRequirement(minUpper, CharacterPools.UpperCase));

        if (minDigits > 0)
            requirements.Add(new CharacterRequirement(minDigits, CharacterPools.Digits));

        if (minSpecial > 0)
            requirements.Add(new CharacterRequirement(minSpecial, CharacterPools.Special));

        return new PasswordSettings(requirements);
    }
}