﻿using AutoMapper;
using IdentityServer4.EntityFramework.Entities;
using IDS.Model.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDS.Service.AutoMapProfile
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<Client, ClientDTOResponse>().ReverseMap();

            CreateMap<ClientGrantType, ClientGrantTypeDTO>().ReverseMap();
            CreateMap<ClientPostLogoutRedirectUri, ClientPostLogoutRedirectUriDTO>().ReverseMap();
            CreateMap<ClientRedirectUri, ClientRedirectUriDTO>().ReverseMap();
            CreateMap<ClientScope, ClientScopeDTO>().ReverseMap();
        }
    }
}