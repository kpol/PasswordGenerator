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

    [Theory]
    [InlineData(6, 12)]
    [InlineData(10, 10)] // exact length
    [InlineData(4, 5)]
    public void GenerateRandomLength_ReturnsPasswordWithinRange(int min, int max)
    {
        var generator = CreateDefaultGenerator();

        string password = generator.GenerateRandomLength(min, max);

        Assert.InRange(password.Length, min, max);
    }

    [Fact]
    public void GenerateRandomLength_Throws_WhenMinGreaterThanMax()
    {
        var generator = CreateDefaultGenerator(); ;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            generator.GenerateRandomLength(10, 5));
    }

    [Fact]
    public void GenerateRandomLength_Throws_WhenMinLessThanRequired()
    {
        var generator = CreateDefaultGenerator();
        int tooSmall = 3;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            generator.GenerateRandomLength(tooSmall, 10));
    }

    [Fact]
    public void Validate_ReturnsTrue_WhenPasswordMeetsAllRequirements()
    {
        var generator = CreateDefaultGenerator();

        // 'a', 'b' (from "abc"), '1' (from "123"), '!' (from "!@#")
        string validPassword = "ab1!xyz";

        Assert.True(generator.Validate(validPassword));
    }

    [Fact]
    public void Validate_ReturnsFalse_WhenPasswordTooShort()
    {
        var generator = CreateDefaultGenerator();

        string shortPassword = "a1!";

        Assert.False(generator.Validate(shortPassword));
    }

    [Fact]
    public void Validate_ReturnsFalse_WhenRequirementNotMet()
    {
        var generator = CreateDefaultGenerator();

        // Missing digits
        string invalidPassword = "ab!!xyz";

        Assert.False(generator.Validate(invalidPassword));
    }

    [Fact]
    public void Validate_Throws_WhenPasswordIsNull()
    {
        var generator = CreateDefaultGenerator();

        Assert.Throws<ArgumentNullException>(() =>
            generator.Validate(null!));
    }

    private static PasswordGenerator CreateDefaultGenerator()
    {
        List<CharacterRequirement> requirements = 
        [
            new CharacterRequirement(2, "abc"),
            new CharacterRequirement(1, "123"),
            new CharacterRequirement(1, "!@#")
        ];

        return new PasswordGenerator(new PasswordSettings(requirements));
    }
}