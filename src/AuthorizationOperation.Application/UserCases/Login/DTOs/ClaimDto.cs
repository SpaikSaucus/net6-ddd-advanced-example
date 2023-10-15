using AuthorizationOperation.Domain.Core;

namespace AuthorizationOperation.Application.UserCases.Login.DTOs
{
    public class ClaimDto : IUserInformation
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
