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
        ArgumentOutOfRangeException.ThrowIfLessThan(passwordLength, _passwordSettings._minimumPasswordLength);

        Span<char> result = passwordLength <= 512 ?
            stackalloc char[passwordLength] :
            new char[passwordLength];

        int index = 0;

        foreach (var charSet in _passwordSettings.CharSets)
        {
            RandomNumberGenerator.GetItems(charSet.Chars, result.Slice(index, charSet.Count));
            index += charSet.Count;
        }

        RandomNumberGenerator.GetItems(_passwordSettings._allChars, result[index..]);
        RandomNumberGenerator.Shuffle(result);

        return result.ToString();
    }
}