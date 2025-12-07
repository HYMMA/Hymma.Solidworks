# Contributing to Hymma.Solidworks

Thank you for your interest in contributing to Hymma.Solidworks! This document provides guidelines and information for contributors.

## Getting Started

### Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- SolidWorks 2018 or later (for testing)

### Setting Up the Development Environment

1. Fork the repository
2. Clone your fork:
   ```bash
   git clone https://github.com/YOUR-USERNAME/Hymma.Solidworks.git
   ```
3. Open `Hymma.Solidworks.sln` in Visual Studio
4. Build the solution to restore NuGet packages

## How to Contribute

### Reporting Bugs

Before creating a bug report, please check existing issues to avoid duplicates.

When filing a bug report, include:

- **SolidWorks version** you're using
- **.NET Framework version**
- **Steps to reproduce** the issue
- **Expected behavior** vs **actual behavior**
- **Error messages** or stack traces if available
- **Code snippets** that demonstrate the issue

### Suggesting Features

Feature requests are welcome! Please provide:

- A clear description of the feature
- Use cases and benefits
- Any relevant examples from other libraries

### Pull Requests

1. Create a feature branch from `dev`:
   ```bash
   git checkout dev
   git pull origin dev
   git checkout -b feature/your-feature-name
   ```

2. Make your changes following the coding standards below

3. Test your changes with SolidWorks

4. Commit with clear, descriptive messages:
   ```bash
   git commit -m "Add: Feature description"
   ```

5. Push to your fork and create a Pull Request against the `dev` branch

## Coding Standards

### C# Style Guidelines

- Use C# 7.3 features (compatible with .NET Framework 4.8)
- Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation comments for public APIs

### Example

```csharp
/// <summary>
/// Gets the active document from SolidWorks.
/// </summary>
/// <param name="app">The SolidWorks application instance.</param>
/// <returns>The active model document, or null if none is open.</returns>
public static ModelDoc2 GetActiveDocument(this ISldWorks app)
{
    if (app == null)
        throw new ArgumentNullException(nameof(app));

    return app.ActiveDoc as ModelDoc2;
}
```

### Project Structure

- `Interop/` - SolidWorks interop references
- `Extensions/` - Extension methods for SolidWorks API
- `Addins/` - Base add-in framework
- `Addins.Fluent/` - Fluent API wrapper
- `QRify/` and `QRifyPlus/` - Sample add-ins

## Testing

### Running Tests

```bash
dotnet test
```

### Writing Tests

- Place tests in the appropriate test project
- Use descriptive test names: `MethodName_Scenario_ExpectedResult`
- Mock SolidWorks interfaces where possible

## Commit Message Format

Use clear, descriptive commit messages:

- `Add:` for new features
- `Fix:` for bug fixes
- `Update:` for changes to existing functionality
- `Docs:` for documentation changes
- `Refactor:` for code refactoring
- `Test:` for test additions or changes

## Code Review Process

1. All PRs require at least one approving review
2. CI checks must pass
3. No merge conflicts with the target branch
4. Documentation updated if needed

## Questions?

Feel free to open an issue for any questions about contributing.

## License

By contributing to Hymma.Solidworks, you agree that your contributions will be licensed under the MIT License.
