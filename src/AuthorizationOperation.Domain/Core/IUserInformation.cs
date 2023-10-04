namespace AuthorizationOperation.Domain.Core
{
    public interface IUserInformation
    {
        string Guid { get; }
        string UserName { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
    }
}
