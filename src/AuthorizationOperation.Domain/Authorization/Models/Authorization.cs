using System;

namespace AuthorizationOperation.Domain.Authorization.Models
{
    public class Authorization
    {
        public uint Id { get; set; }
        public Guid UUID { get; set; }
        public string Customer { get; set; }
        public AuthorizationStatusEnum StatusId { get; set; }
        public AuthorizationStatus Status { get; set; }
        public DateTime Created { get; set; }
    }
}
