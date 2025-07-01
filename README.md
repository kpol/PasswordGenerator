# PasswordGenerator

A lightweight, secure, and flexible password generator library for .NET, built with cryptographic randomness and fully customizable character sets.

[![Build & Test](https://github.com/kpol/PasswordGenerator/actions/workflows/build-test.yml/badge.svg)](https://github.com/kpol/PasswordGenerator/actions)
[![NuGet](https://img.shields.io/nuget/v/KPasswordGenerator.svg)](https://www.nuget.org/packages/KPasswordGenerator)

---

## Features

- **Cryptographically secure** — built on .NET’s [`RandomNumberGenerator`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator) for high-entropy randomness
- **Performance-focused** — uses `Span<char>` and stack allocation to minimize heap usage and avoid unnecessary string allocations
- **Customizable character sets** — define your own character pools (e.g., symbols, digits, uppercase)
- **Per-set minimum requirements** — ensure that a minimum number of characters from each set are included
- **Predictable-free output** — final password is securely shuffled to eliminate character order patterns
- **Ambiguity-avoiding** — easily exclude confusing characters like `0`, `O`, `l`, and `I`
- Clean, testable design — .NET 8 compatible and unit-tested


---

```csharp
using KPasswordGenerator;

// Define your password policy
PasswordSettings settings = new(
[
    new CharacterRequirement(minRequired: 2, characterPool: "ABCDEFGHJKLMNPQRSTUVWXYZ"),
    new CharacterRequirement(3, "abcdefghijkmnopqrstuvwxyz"),   // At least 3 lowercase letters (no l)
    new CharacterRequirement(4, "23456789"),                    // At least 4 digits (no 0, 1)
    new CharacterRequirement(2, "!@$?_-")                       // At least 2 symbols
]);

PasswordGenerator generator = new(settings);

string password = generator.Generate(16);

Console.WriteLine(password);  // Example output: kAj79uV@E?m7_8eS
```

### Using WithDefaults
Quickly generate a password using default pools:
```csharp
PasswordSettings settings = PasswordSettings.WithDefaults(minLower: 2, minUpper: 2, minDigits: 2, minSpecial: 1);
PasswordGenerator generator = new(settings);

string password = generator.Generate(12);
```

## Additional Methods

### `GenerateRandomLength(int minPasswordLength, int maxPasswordLength)`

Generates a password of **random length within a specified range**.

- Ensures the generated password meets all defined character requirements.
- Uses secure randomness to choose the length.

**Example:**
```csharp
string password = generator.GenerateRandomLength(12, 20);
```

---

### `Validate(string password)`

Validates whether a password meets the current `PasswordSettings` policy.

- Checks overall length.
- Verifies that the required number of characters from each character pool are present.
- Returns `true` if all rules pass, otherwise `false`.

- **Example:**
```csharp
bool isValid = generator.Validate("abcD123!");
```

## Installation

Install via NuGet:

```bash
dotnet add package KPasswordGenerator
