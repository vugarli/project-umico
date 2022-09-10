using AutoMapper;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Mapings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category,CategoryDto>()
            .ForMember(dest=>
                dest.CategoryName,opt=>
                opt.MapFrom(orig=>orig.Name))
            .ForMember(dest=>
                dest.CategoryId,opt=>
                opt.MapFrom(orig=>orig.Id))
            .ForMember(dest=>
                dest.CategoryParentName,opt=>
                opt.MapFrom(orig=>orig.Parent.Name));
        
        CreateMap<CategoryDto,Category>();

        

    }
}