using ST.Core.Identity.Testing.Toolkit.Models;
using System.Diagnostics.CodeAnalysis;

namespace ST.Core.Identity.Testing.Toolkit.Builders;

/// <summary>
/// Provides a builder for creating <see cref="TestUser"/> instances for testing purposes.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestUserBuilder
{
    private readonly TestUser _user = new();

    /// <summary>
    /// Sets the user name for the test user.
    /// </summary>
    /// <param name="userName">The user name to assign.</param>
    /// <returns>The current <see cref="TestUserBuilder"/> instance.</returns>
    public TestUserBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        return this;
    }

    /// <summary>
    /// Sets the email address for the test user.
    /// </summary>
    /// <param name="email">The email address to assign.</param>
    /// <returns>The current <see cref="TestUserBuilder"/> instance.</returns>
    public TestUserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    /// <summary>
    /// Sets the roles for the test user.
    /// </summary>
    /// <param name="roles">The roles to assign.</param>
    /// <returns>The current <see cref="TestUserBuilder"/> instance.</returns>
    public TestUserBuilder WithRoles(params string[] roles)
    {
        _user.Roles = roles.ToList();
        return this;
    }

    /// <summary>
    /// Sets whether the test user's email is confirmed.
    /// </summary>
    /// <param name="confirmed">True if the email is confirmed; otherwise, false.</param>
    /// <returns>The current <see cref="TestUserBuilder"/> instance.</returns>
    public TestUserBuilder WithEmailConfirmed(bool confirmed)
    {
        _user.EmailConfirmed = confirmed;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="TestUser"/> instance.
    /// </summary>
    /// <returns>The constructed <see cref="TestUser"/>.</returns>
    public TestUser Build() => _user;
}