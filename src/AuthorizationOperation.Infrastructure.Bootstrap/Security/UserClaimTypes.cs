namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    /// <summary>
    /// Check if it does not exist in this list: https://learn.microsoft.com/es-es/dotnet/api/system.security.claims.claimtypes?view=net-8.0 
    /// If it exists, the collision means that the custom type is replaces with http://schemas.xmlsoap.org/ws/2005/05/identity/claims/[text] 
    /// and throws "ErrorClaimNotExist" when you run the constructor for ClaimsBaseUser.
    /// </summary>
    public class UserClaimTypes
    {
        public const string Guid = "guid";
    }
}
