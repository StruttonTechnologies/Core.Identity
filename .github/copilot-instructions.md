# Git Copilot Guidelines

This repository contains development guidelines used to help guide GitHub Copilot when generating code for repositories within the Strutton Technologies ecosystem.

These guidelines are intentionally simple and explicit so that Copilot can generate code that aligns with the preferred development patterns used in these projects.

As new preferences or patterns are identified, they can be added to this document and incorporated into project-level Copilot instruction files.

---

# Purpose

AI-assisted code generation can produce large amounts of code quickly. Without guidance, generated code may not match the development style or architectural expectations used within a project.

The purpose of these guidelines is to ensure that generated code:

- follows consistent coding practices
- aligns with established architectural patterns
- remains readable and maintainable
- integrates well with existing solutions

---

# Core Coding Preferences

The following rules describe the preferred coding style for generated code.

These rules should be followed whenever possible when generating C# code.

## Namespace Style

Use **block-scoped namespaces**.

Example:

```csharp
namespace Example.Project.Domain
{
    public class ExampleClass
    {
    }
}
```

Do **not** use file-scoped namespaces.

Avoid:

```csharp
namespace Example.Project.Domain;
```

---

## Braces for Control Statements

Always use braces for control statements, even if the body contains a single line.

Preferred:

```csharp
if (condition)
{
    DoWork();
}
```

Avoid:

```csharp
if (condition)
    DoWork();
```

Using braces improves readability and reduces the chance of errors when code is modified later.

---

## General Principles

Generated code should aim to be:

- clear
- readable
- maintainable
- consistent with the surrounding codebase

Avoid overly clever implementations. Favor straightforward solutions that other developers can easily understand.

---

# Expanding These Guidelines

This document will evolve over time as additional development patterns and preferences are identified.

Future additions may include guidance related to:

- architecture layering
- repository structure
- testing expectations
- naming conventions
- project organization

---

---

## File Organization

Avoid creating large "mega classes" that contain many unrelated methods.

Instead, use **partial classes and method-per-file organization** to keep code easy to navigate and maintain.

### Parent Class Pattern

Create a parent partial class that represents the logical class.

Example:

```csharp
namespace Example.Project.Guards
{
    public static partial class Guard
    {
    }
}
```

This file acts as the logical root for the class.

---

### Method Organization

Place methods into separate files using partial classes.

Create a folder that groups the methods. The folder name should either be:

- `Methods`
- or a descriptive category name when appropriate

Example structure:

```
Guard
├─ Guard.cs
└─ Methods
   ├─ Guard.AgainstNull.cs
   ├─ Guard.AgainstEmpty.cs
   └─ Guard.AgainstOutOfRange.cs
```

Each method file should declare the same partial class.

Example:

```csharp
namespace Example.Project.Guards
{
    public static partial class Guard
    {
        public static void AgainstNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
```

---

### Namespace Consistency

All partial class files must use the **same namespace** as the parent class.

Do not create nested or alternate namespaces for method files.

Example:

```
namespace Example.Project.Guards
```

Consistency is important so the compiler correctly merges the partial class definitions.

---

---

## Test Organization

Unit tests should be organized to keep tests easy to navigate and maintain.

Avoid creating a single large test class containing many unrelated tests.

Instead, organize tests by **class under test** and group related scenarios together.

---

### Folder Structure

Create a folder for the class being tested.

Example:

```
GuardTests
└─ AgainstNull
```

This folder contains tests related to the `Guard.AgainstNull` behavior.

---

### File Organization

Within the folder, create **test files grouped by scenario or behavior**.

Each file may contain multiple related tests.

Example:

```
GuardTests
└─ AgainstNull
   ├─ NullValueTests.cs
   ├─ ValidValueTests.cs
   └─ ExceptionBehaviorTests.cs
```

This keeps tests organized without creating excessive numbers of files.

---

### Example Test File

Example structure for a grouped test file:

```csharp
public class NullValueTests
{
    [Fact]
    public void ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Guard.AgainstNull(null, "value"));
    }

    [Fact]
    public void ExceptionShouldContainCorrectParameterName()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            Guard.AgainstNull(null, "value"));

        Assert.Equal("value", exception.ParamName);
    }
}
```

---

### Naming Guidelines

Test class names should describe the behavior being tested.

Examples:

- `NullValueTests`
- `ValidValueTests`
- `ExceptionBehaviorTests`

Avoid generic names such as:

- `Tests`
- `GuardTests`
- `TestCases`

---

### Goals of This Structure

This approach helps ensure tests are:

- organized by behavior
- easy to locate
- easy to expand
- easier for AI tools to extend without modifying large files

Favor **scenario-based groupings** over extremely large test files or one-file-per-test structures.