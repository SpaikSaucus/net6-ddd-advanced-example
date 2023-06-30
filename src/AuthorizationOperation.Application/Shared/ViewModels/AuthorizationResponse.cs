using System;

namespace AuthorizationOperation.Application.Shared.ViewModels
{
    public class AuthorizationResponse
    {
        public uint Id { get; set; }

        public Guid UUID { get; set; }
        
        public string Customer { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }
    }
}
