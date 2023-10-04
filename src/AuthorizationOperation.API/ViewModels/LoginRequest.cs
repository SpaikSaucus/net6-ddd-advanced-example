using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AuthorizationOperation.API.ViewModels
{
    public class LoginRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>userName</example>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>my_password</example>
        public string password { get; set; }
    }
}
