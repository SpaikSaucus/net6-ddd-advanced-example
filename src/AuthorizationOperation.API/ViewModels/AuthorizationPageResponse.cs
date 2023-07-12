using AuthorizationOperation.Application.Shared.DTO;
using AuthorizationOperation.Domain.Authorization.Models;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationOperation.API.ViewModels
{
    /// <summary>
    /// </summary>
    public class AuthorizationPageResponse
    {
        public AuthorizationPageResponse(PageDto<Authorization> dto) 
        {
            this.Authorizations = dto.Items.Select(x => new AuthorizationResponse(x));
            this.Offset = dto.Offset;
            this.Total = dto.Total;
            this.Limit = dto.Limit;
        }

        /// <summary>
        /// Total result.
        /// </summary>
        /// <example>1</example>
        public int Total { get; set; }

        /// <summary>
        /// Page result (0..N).
        /// </summary>
        /// <example>0</example>
        public uint Offset { get; set; }

        /// <summary>
        /// Limit per page.
        /// </summary>
        /// <example>200</example>
        public ushort Limit { get; set; }

        /// <summary>
        /// List authorizations.
        /// </summary>
        public IEnumerable<AuthorizationResponse> Authorizations { get; set; }
    }
}
