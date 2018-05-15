using System;

namespace ClientList.Models
{
    public class UserModel
    {
        public Guid ClientId { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
