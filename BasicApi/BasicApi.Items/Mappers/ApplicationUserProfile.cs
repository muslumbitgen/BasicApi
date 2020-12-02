using AutoMapper;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Mappers
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<CreateApplicationUserCommand, ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            });
            CreateMap<ApplicationUserViewDto, ApplicationUser>().AfterMap((src, dest) => { });

        }
    }
}
