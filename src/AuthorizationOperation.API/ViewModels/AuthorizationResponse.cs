using AuthorizationOperation.Domain.Authorization.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationOperation.API.ViewModels
{
    /// <summary>
    /// </summary>
    public class AuthorizationResponse
    {
        public AuthorizationResponse(Authorization entity) 
        {
            this.Id = entity.Id;
            this.UUID = entity.UUID;
            this.Customer = entity.Customer;
            this.Status = entity.Status.Name.ToString();
            this.Created = entity.Created;
        }

        /// <summary>
        /// Id of the authorization.
        /// </summary>
        /// <example>1111</example>
        public uint Id { get; set; }
        /// <summary>
        /// Guid of the authorization.
        /// </summary>
        /// <example>4bac8878-d319-4a8d-9648-87da3fbf2cc7</example>
        public Guid UUID { get; set; }
        /// <summary>
        /// Customer of the authorization.
        /// </summary>
        /// <example>Customer1</example>
        [Required]
        public string Customer { get; set; }
        /// <summary>
        /// Status of the authorization.
        /// Available values: WAITING_FOR_SIGNERS | AUTHORIZED | EXPIRED | CANCELLED
        /// </summary>
        /// <example>WAITING_FOR_SIGNERS</example>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// Created At.
        /// </summary>
        /// <example>2023-07-11T10:15:00-03:00</example>
        public DateTime Created { get; set; }
    }
}
