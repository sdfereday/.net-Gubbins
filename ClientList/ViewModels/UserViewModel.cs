using System;
using System.Collections.Generic;

namespace ClientList.ViewModels
{
    public class UserViewModel
    {
        public Guid ClientId { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClientViewModel> Clients { get; set; }
    }
}
