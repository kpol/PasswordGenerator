namespace KPasswordGenerator.Tests;

public class PasswordGeneratorTests
{
    private static readonly CharacterRequirement _lowerCase = new(2, "abcdef");
    private static readonly CharacterRequirement _upperCase = new(2, "ABCDEF");
    private static readonly CharacterRequirement _digits = new(2, "012345");

    private static PasswordSettings CreateSettings() =>
        new([_lowerCase, _upperCase, _digits]);

    [Fact]
    public void Constructor_ThrowsOnNullSettings()
    {
        Assert.Throws<ArgumentNullException>(() => new PasswordGenerator(null!));
    }

    [Fact]
    public void Generate_ThrowsIfLengthBelowMinimum()
    {
        var settings = CreateSettings(); // min length = 6
        var generator = new PasswordGenerator(settings);

        Assert.Throws<ArgumentOutOfRangeException>(() => generator.Generate(5));
    }

    [Fact]
    public void Generate_ReturnsPasswordOfCorrectLength()
    {
        var settings = CreateSettings(); // min = 6
        var generator = new PasswordGenerator(settings);

        string password = generator.Generate(10);

        Assert.Equal(10, password.Length);
    }

    [Fact]
    public void Generate_UsesAtLeastRequiredCountFromEachCharSet()
    {
        var settings = CreateSettings(); // Each has Count = 2
        var generator = new PasswordGenerator(settings);

        string password = generator.Generate(10);

        // Ensure at least 2 of each set
        Assert.True(password.Count(_lowerCase.CharacterPool.Contains) >= _lowerCase.MinRequired);
        Assert.True(password.Count(_upperCase.CharacterPool.Contains) >= _upperCase.MinRequired);
        Assert.True(password.Count(_digits.CharacterPool.Contains) >= _digits.MinRequired);
    }

    [Fact]
    public void Generate_DoesNotReturnSamePasswordEveryTime()
    {
        var settings = CreateSettings();
        var generator = new PasswordGenerator(settings);

        var password1 = generator.Generate(10);
        var password2 = generator.Generate(10);

        Assert.NotEqual(password1, password2); // May rarely fail on pure randomness
    }
}