using AuthorizationOperation.Application.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindAll.ViewModels
{
    public class AuthorizationPageResponse
    {
        public int Total { get; set; }

        public uint Offset { get; set; }

        public ushort Limit { get; set; }

        public IList<AuthorizationResponse> Authorizations { get; set; }
    }
}
