using AutoMapper;
using BasicApi.Items.Commands;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Mappers
{
    public class ApplicationRoleProfile : Profile
    {
        public ApplicationRoleProfile()
        {
            CreateMap<CreateApplicationRoleCommand, ApplicationRole>().AfterMap((src, dest) => { });

        }
    }
}
