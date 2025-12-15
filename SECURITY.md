# Security Policy

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.x.x   | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability in Hymma.Solidworks, please report it responsibly.

### How to Report

1. **Do not** open a public GitHub issue for security vulnerabilities
2. Email the maintainers directly or use GitHub's private vulnerability reporting feature
3. Include:
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Any suggested fixes (optional)

### What to Expect

- **Acknowledgment**: Within 48 hours
- **Initial Assessment**: Within 1 week
- **Resolution Timeline**: Depends on severity, typically 30-90 days

### Scope

This security policy covers:
- The core library packages (Interop, Extensions, Addins, Addins.Fluent)
- Sample add-ins (QRify, QRifyPlus)

### Out of Scope

- Vulnerabilities in SolidWorks itself
- Issues in third-party dependencies (report to respective maintainers)
- Social engineering attacks

## Security Best Practices for Users

When using Hymma.Solidworks in your add-ins:

1. **Sign your assemblies** with a strong name key
2. **Validate user input** before processing
3. **Use secure coding practices** for file operations
4. **Keep dependencies updated** to patch known vulnerabilities
5. **Register COM objects** with appropriate permissions
