using AutoMapper;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Dtos;
using umico.Models;
using umico.Models.Categories;
using umico.Models.Rating;

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
        
        CreateMap<CategoryDto,Category>()
            .ForPath(dest=> dest.Parent.Name,
                opt=>opt.MapFrom(orig=>orig.CategoryParentName));

        CreateMap<ProductRating, ProductRatingDto>();
        CreateMap<Promotion,PromotionDto>();
        CreateMap<ProductAtribute,ProductAtributeDto>();
        
        //CreateMap<CompanyProductSaleEntry, CompanyProductSaleEntryDto>();
        
        CreateMap<ProductDto,Product>();
        CreateMap<Product,ProductDto>();


    }
}