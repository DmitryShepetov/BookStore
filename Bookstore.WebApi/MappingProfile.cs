using AutoMapper;
using Bookstore.WebApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.WebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistration, User>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
