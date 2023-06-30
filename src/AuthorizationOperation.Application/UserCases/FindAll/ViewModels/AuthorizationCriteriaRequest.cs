using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizationOperation.Application.Shared.ViewModels;

namespace AuthorizationOperation.Application.UserCases.FindAll.ViewModels
{
    public class AuthorizationCriteriaRequest
    {
        public List<EnumStatusRequest> listStatus { get; set; }
    }
}
