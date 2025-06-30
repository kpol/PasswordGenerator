using System.Security.Cryptography;

namespace KPasswordGenerator;

public sealed class PasswordGenerator
{
    private readonly PasswordSettings _passwordSettings;

    public PasswordGenerator(PasswordSettings passwordSettings)
    {
        ArgumentNullException.ThrowIfNull(passwordSettings);

        _passwordSettings = passwordSettings;
    }

    public string Generate(int passwordLength)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(passwordLength, _passwordSettings.MinimumPasswordLength);

        Span<char> buffer = passwordLength <= 512 ?
            stackalloc char[passwordLength] :
            new char[passwordLength];

        int index = 0;

        foreach (var requirement in _passwordSettings.CharacterRequirements)
        {
            RandomNumberGenerator.GetItems(requirement.CharacterPool, buffer.Slice(index, requirement.MinRequired));
            index += requirement.MinRequired;
        }

        RandomNumberGenerator.GetItems(_passwordSettings.AllChars, buffer[index..]);
        RandomNumberGenerator.Shuffle(buffer);

        return buffer.ToString();
    }
}