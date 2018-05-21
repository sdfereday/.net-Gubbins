using System;
using System.Collections.Generic;
using ClientList.Features.User.ViewModels;

namespace ClientList.Features.Client.ViewModels
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
