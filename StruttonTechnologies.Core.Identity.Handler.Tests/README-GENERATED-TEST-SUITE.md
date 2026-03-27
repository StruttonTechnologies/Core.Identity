# Generated coordinator test suite

This package adds a broad handler-focused unit test suite for the current uploaded snapshot of `StruttonTechnologies.Core.Identity.Coordinator`.

## Covered handlers
- Authentication: authenticate, change password, confirm email, forgot password, register, reset password, external login, link external login, unlink external login, sign out
- Authorization: add claim, assign role, remove role, get claims principal, get user claims, get user roles
- JWT tokens: generate token, refresh token, revoke token
- Users: create user, disable user, enable user, get all users, get normalized email, get user by email, get user logins, get user profile

## Not covered in this snapshot
These handlers are currently empty placeholder classes in the uploaded source, so they are not meaningfully testable yet:
- `GetUserByIdQueryHandler`
- `LockUserCommandHandler`
- `UnlockUserCommandHandler`
- `UpdateUserCommandHandler`

## Notes
- Every generated test class includes `[ExcludeFromCodeCoverage]`.
- Internal handlers are tested through reflection so the suite does not require `InternalsVisibleTo`.
- The source zip available here reflects the uploaded snapshot. If your local solution has changed since upload, you may need to reconcile namespaces or project references before running the tests.
