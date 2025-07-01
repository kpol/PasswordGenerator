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

    public string GenerateRandomLength(int minPasswordLength, int maxPasswordLength)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minPasswordLength, maxPasswordLength);
        ArgumentOutOfRangeException.ThrowIfLessThan(minPasswordLength, _passwordSettings.MinimumPasswordLength);

        int length = RandomNumberGenerator.GetInt32(minPasswordLength, maxPasswordLength + 1);

        return Generate(length);
    }

    public bool Validate(string password)
    {
        ArgumentNullException.ThrowIfNull(password);

        if (password.Length < _passwordSettings.MinimumPasswordLength) return false;

        foreach (var requirement in _passwordSettings.CharacterRequirements)
        {
            int count = 0;

            for (int i = 0; i < password.Length && count < requirement.MinRequired; i++)
            {
                if (requirement.CharacterPool.Contains(password[i]))
                {
                    count++;
                }
            }

            if (count != requirement.MinRequired) return false;
        }

        return true;
    }
}