//<<<<<<< HEAD
//﻿using AutoMapper;
//using Demo.Data;
//using Demo.ModelViews;

//namespace Demo.Helper
//{
//    public class AutoMapperProfile : Profile
//    {
//        public AutoMapperProfile() {
//            CreateMap<RegisterViewModel, Customer>();

//        }
//    }
//}
//=======
﻿using AutoMapper;
using Demo.Data;
using Demo.ModelViews;

namespace Demo.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<RegisterViewModel, Customer>();

        }
    }
}
//>>>>>>> 1ff40e56d8e6dd36d58c1a78e757dc1ed9ee2228
