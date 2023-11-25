using System;

namespace AuthorizationOperation.Domain.User.Models
{
    public class User
    {
        public User() { }
        public User(Guid uuid, string userName, string password, string email) 
        {
            this.UUID = uuid;
            this.UserName = userName;
            this.Password = password;
            this.Email = email;
            this.Created = DateTime.Now;
        }

        public Guid UUID{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Created { get; set; }
    }
}
