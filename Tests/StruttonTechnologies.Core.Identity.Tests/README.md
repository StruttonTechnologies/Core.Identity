# StruttonTechnologies.Core.Identity.Tests

This project consolidates the prior Core.Identity test projects into a single test assembly.

## Engineering notes

The previous test layout split tests by implementation package (`Core`, `EF`, `Fakes`, `Handler`, `Infrastructure`, and `Orchestration`). That was workable while the package boundaries were changing, but it increased package-reference duplication, made central package management harder, and allowed some tests to drift out of sync with the authentication DTO and refresh-token requirements.

The consolidated project keeps the logical folder boundaries while compiling and running as one test assembly:

- `Core/` — validators, exceptions, and core identity helpers.
- `EntityFramework/` — EF-backed stores and context validation.
- `Fakes/` — tests for reusable fake data/builders/factories from Core.Testing.
- `Handlers/` — coordinator command/query handler tests.
- `Infrastructure/` — JWT token manager and configuration tests.
- `Orchestration/` — orchestration and token mapping tests.

## Senior engineering assessment

The existing tests are broadly appropriate for the Core.Identity solution. They exercise the right layers: validators, token manager, orchestration, EF stores, coordinators, and test support packages. The main problems were structural rather than conceptual:

1. The test projects were too fragmented for the current size of the solution.
2. Several tests were stale after the access-token + refresh-token response contract changed.
3. Package references were duplicated across projects.
4. Some tests verified old `TokenResponseDto.ExpiresAt` behavior instead of the current access/refresh expiration shape.
5. There is still a gap between unit/integration tests and true API-level acceptance tests.

## Next recommended step

After Core.Identity, Core.PresentationLayer, and a consumer API stabilize, add API-level acceptance tests using `WebApplicationFactory`. Those should validate the public contract:

- Register user.
- Authenticate and receive access + refresh tokens.
- Refresh token rotates tokens.
- Sign out revokes the active refresh token.
- Sign out all devices revokes all refresh tokens.
- Protected endpoint accepts the access token.
- Revoked/expired tokens fail.
