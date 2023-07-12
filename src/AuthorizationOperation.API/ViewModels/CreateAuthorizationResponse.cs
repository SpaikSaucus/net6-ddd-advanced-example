using AuthorizationOperation.Domain.Authorization.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationOperation.API.ViewModels
{
    /// <summary>
    /// </summary>
    public class CreateAuthorizationResponse
    {
        public CreateAuthorizationResponse(uint id) 
        { 
            this.Id = id;
            this.Status = AuthorizationStatusEnum.WAITING_FOR_SIGNERS.ToString();
        }

        /// <summary>
        /// Id of the authorization.
        /// </summary>
        /// <example>1111</example>
        public uint Id { get; set; }

        /// <summary>
        /// Status of the authorization.
        /// Available values: WAITING_FOR_SIGNERS | AUTHORIZED | EXPIRED | CANCELLED
        /// </summary>
        /// <example>WAITING_FOR_SIGNERS</example>
        [Required]
        public string Status { get; set; }
    }
}
