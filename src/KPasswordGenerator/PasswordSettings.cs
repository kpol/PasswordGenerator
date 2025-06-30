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
}