using System;

namespace AuthorizationOperation.Domain.User.Models
{
    public class User
    {
        public Guid UUID{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
