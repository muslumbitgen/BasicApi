using AutoMapper;
using BasicApi.Items.Commands;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Mappers
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<CreateFeedbackCommand, Feedback>().AfterMap((src, dest) =>
            {
                dest.Id = Guid.NewGuid();
            });

        }
    }
}
