using System;
using System.Collections.Generic;
using AutoMapper;

namespace ClientList.Mapping
{
    public class ClientMap : Profile
    {
        public ClientMap()
        {
            CreateMap<ViewModels.ClientViewModel, Models.ClientModel>();
            CreateMap<ViewModels.UserViewModel, Models.UserModel>();
        }
    }    
}
