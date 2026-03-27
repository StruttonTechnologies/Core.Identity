This package is a best-effort refactor of the uploaded Core.Identity solution snapshot.

Included changes:
- Moved Coordinator commands and queries into StruttonTechnologies.Core.Identity.Coordinator.Contracts.
- Removed Coordinator project references to API and Orchestration implementation projects.
- Renamed remaining Dispatcher references to Coordinator.
- Reworked external login handling to use orchestration contracts and a validator abstraction instead of throwing NotImplementedException.
- Replaced static in-memory access-token revocation state in JwtUserTokenManager with a persistent revocation-store abstraction and EF-backed store/entity.
- Added/generated handler test suite files and marked test classes with [ExcludeFromCodeCoverage].

Notes:
- This environment did not have the dotnet SDK available, so the solution was not compile-verified here.
- The repo was zipped without .git, .vs, bin, and obj folders.
