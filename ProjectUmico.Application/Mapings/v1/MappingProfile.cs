using System.Runtime.CompilerServices;
using AutoMapper;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Contracts.Attributes.v1.Commands;
using ProjectUmico.Application.Contracts.Products.v1.Commands;
using ProjectUmico.Application.Contracts.SaleEntries.v1.Commands;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Application.Products.Commands;
using ProjectUmico.Application.Products.v1.Commands;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;
using umico.Models.Categories;
using umico.Models.Rating;

namespace ProjectUmico.Application.Mapings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(dest =>
                dest.CategoryName, opt =>
                opt.MapFrom(orig => orig.Name))
            .ForMember(dest =>
                dest.CategoryId, opt =>
                opt.MapFrom(orig => orig.Id))
            .ForMember(dest =>
                dest.CategoryParentName, opt =>
                opt.MapFrom(orig => orig.Parent.Name));

        CreateMap<CategoryDto, Category>()
            .ForPath(dest => dest.Parent.Name,
                opt => opt.MapFrom(orig => orig.CategoryParentName))
            .ForPath(dest => dest.Id, opt => opt.MapFrom(d => d.CategoryId))
            .ForPath(dest => dest.ParentId, opt => opt.MapFrom(d => d.ParentId))
            .ForPath(dest => dest.Name, opt => opt.MapFrom(d => d.CategoryName));

        CreateMap<ProductRating, ProductRatingDto>()
            .ForPath(dest=>dest.Rate,
                opt=>
                    opt.MapFrom(src=>src.Rate));
        
        CreateMap<Promotion, PromotionDto>();
        CreateMap<ProductAttribute, AttributeDto>();

        CreateMap<AddAttributeCommandV1.AddAttributeCommand, ProductAttribute>();
        CreateMap<UpdateAttributeCommandV1.UpdateAttributeCommand, ProductAttribute>();
    

        //CreateMap<CompanyProductSaleEntry, CompanyProductSaleEntryDto>();
        // CreateMap<ProductDto,Product>().ForPath(dest=>dest.CategoryId,opt=>opt.MapFrom(p=>p.CategoryId));

        CreateMap<AddProductCommandV1.AddProductCommand, Product>();

        // CreateMap<AddAttributeToProductCommandV1.AddAttributeToProductCommand, ProductAttribute>()
        // .IncludeMembers(a=>a.attribute);
        
        CreateMap<Product, ProductDto>();

        // CreateMap<AddAttributeToProductCommandV1.AddAttribute, ProductAttribute>()
        //     .ForAllMembersEnforceCustomDefaultValues();

        CreateMap<UpdateProductCommandV1.UpdateProductCommand, Product>()
            .ForAllMembersEnforceCustomDefaultValues();
        
        CreateMap<AddRatingToProductCommandV1.AddRatingToProductCommand, ProductRating>()
            .ForAllMembersEnforceCustomDefaultValues();

        CreateMap<AddSaleEntryCommandV1.AddSaleEntryCommand, CompanyProductSaleEntry>();

        CreateMap<CompanyProductSaleEntry,CompanyProductSaleEntryDto>();

    }
}