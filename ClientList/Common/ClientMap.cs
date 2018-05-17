using System;
using System.Collections.Generic;
using AutoMapper;

namespace ClientList.Mapping
{
    public class ClientMap : Profile
    {
        public ClientMap()
        {
            CreateMap<Models.ClientModel, ViewModels.ClientViewModel>();
            CreateMap<Models.UserModel, ViewModels.UserViewModel>()
                .ForMember(x => x.Clients, opt => opt.Ignore());
        }
    }    
}
