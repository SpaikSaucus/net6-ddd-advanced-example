using System;

namespace AuthorizationOperation.Application.UserCases.Create.ViewModels
{
    public class CreateAuthorizationRequest
    {
        public Guid UUID { get; set; }

        public string Customer { get; set; }
    }
}
