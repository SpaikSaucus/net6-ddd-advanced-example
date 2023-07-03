using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationOperation.Domain.Authorization.Models
{
    public class AuthorizationStatus
    {
        public AuthorizationStatusEnum Id { get; set; }
        public string Name { get; set; }
    }

    public enum AuthorizationStatusEnum
    {
        WAITING_FOR_SIGNERS,
        AUTHORIZED,
        EXPIRED,
        CANCELLED
    }
}
