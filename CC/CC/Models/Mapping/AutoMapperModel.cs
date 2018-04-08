using CC.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CC.Context.ContextModels;

namespace CC.Models
{
    public class AutoMapperModel : Profile
    {
        public AutoMapperModel()
        {
            CreateMap<User, UserCreateModel>();
            CreateMap<UserCreateModel, User>();

            CreateMap<User, UserEditPasswordModel>();
            CreateMap<UserEditPasswordModel, User>();

            CreateMap<User, UserGetCoins>();
            CreateMap<UserGetCoins, User>();

            CreateMap<User, UserGetRightsModel>();
            CreateMap<UserGetRightsModel, User>();
        }
    }
}