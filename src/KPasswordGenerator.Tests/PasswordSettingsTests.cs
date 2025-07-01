namespace KPasswordGenerator.Tests;

public class PasswordSettingsTests
{
    [Fact]
    public void WithDefaults_UsesExpectedDefaults()
    {
        var settings = PasswordSettings.WithDefaults();

        Assert.Equal(4, settings.CharacterRequirements.Count);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == 1 && r.CharacterPool == CharacterPools.LowerCase);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == 1 && r.CharacterPool == CharacterPools.UpperCase);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == 1 && r.CharacterPool == CharacterPools.Digits);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == 1 && r.CharacterPool == CharacterPools.Special);
    }

    [Fact]
    public void WithDefaults_AllZero_ReturnsEmptyRequirements()
    {
        var settings = PasswordSettings.WithDefaults(0, 0, 0, 0);

        Assert.Empty(settings.CharacterRequirements);
    }

    [Fact]
    public void WithDefaults_CreatesRequirementsWithCorrectValues()
    {
        int lower = 2, upper = 3, digits = 4, special = 5;

        var settings = PasswordSettings.WithDefaults(lower, upper, digits, special);

        Assert.Equal(4, settings.CharacterRequirements.Count);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == lower && r.CharacterPool == CharacterPools.LowerCase);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == upper && r.CharacterPool == CharacterPools.UpperCase);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == digits && r.CharacterPool == CharacterPools.Digits);
        Assert.Contains(settings.CharacterRequirements, r =>
            r.MinRequired == special && r.CharacterPool == CharacterPools.Special);
    }
}