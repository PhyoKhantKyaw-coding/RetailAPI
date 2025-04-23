using AutoMapper;
using MODEL.DTOs;
using MODEL.Entities;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, AddProductDTO>().ReverseMap();   
            CreateMap<Product, UpdateProductDTO>();
            CreateMap<UpdateProductDTO, Product>();

        }
    }
}
