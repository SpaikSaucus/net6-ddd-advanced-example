using System;

namespace AuthorizationOperation.Domain.Authorization.Models
{
    public class Authorization
    {
        public Authorization() { }
        public Authorization(uint id , Guid uuid, string customer, AuthorizationStatusEnum statusId)
        { 
            this.Id = id;
            this.UUID = uuid;
            this.Customer = customer;
            this.StatusId = statusId;
            this.Created = DateTime.UtcNow;
        }

        public uint Id { get; set; }
        public Guid UUID { get; set; }
        public string Customer { get; set; }
        public AuthorizationStatusEnum StatusId { get; set; }
        public AuthorizationStatus Status { get; set; }
        public DateTime Created { get; set; }
    }
}
