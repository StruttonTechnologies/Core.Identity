namespace ST.Core.Identity.Dtos.Authentication
{
    /// <summary>
    /// Represents the input payload for authentication validation.
    /// Used to validate session, tenant, and scope context before issuing tokens or granting access.
    /// </summary>
    public class AuthenticationContext
    {
        /// <summary>
        /// The session identifier associated with the authentication request.
        /// Must be a non-empty GUID string.
        /// </summary>
        public string SessionId { get; set; } = String.Empty;

        /// <summary>
        /// The tenant identifier under which the authentication is scoped.
        /// Must be a non-empty GUID string.
        /// </summary>
        public string TenantId { get; set; } = String.Empty;

        /// <summary>
        /// The list of scopes requested by the client.
        /// Each scope must be recognized and authorized.
        /// </summary>
        public IEnumerable<string> Scopes { get; set; } = Array.Empty<string>();
    }
}
