using AutoMapper;
using RESTFulSocial.Core.DTOs;
using RESTFulSocial.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTFulSocial.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
        }
    }
}
