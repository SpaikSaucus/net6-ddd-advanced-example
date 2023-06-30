using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationOperation.Domain.Authorization.Models
{
    public enum AuthorizationStatus
    {
        WAITING_FOR_SIGNERS,
        AUTHORIZED,
        EXPIRED,
        CANCELLED
    }
}
