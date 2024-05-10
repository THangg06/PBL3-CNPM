using AutoMapper;
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
