using System;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationOperation.API.ViewModels
{
    /// <summary>
    /// </summary>
    public class CreateAuthorizationRequest
    {
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
    }
}
