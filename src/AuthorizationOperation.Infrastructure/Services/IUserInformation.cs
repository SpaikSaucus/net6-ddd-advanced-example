namespace AuthorizationOperation.Infrastructure.Services
{
    public interface IUserInformation
    {
        string UserName { get; }

        string Password { get; }

        string Branch { get; }
    }
}
