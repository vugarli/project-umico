﻿using AutoMapper;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Contracts.Attributes.v1.Commands;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Application.Products.Commands;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;
using umico.Models.Categories;
using umico.Models.Rating;
using Attribute = ProjectUmico.Domain.Models.Attributes.Attribute;

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
                opt=>opt.MapFrom(orig=>orig.CategoryParentName))
            .ForPath(dest=>dest.Id,opt=>opt.MapFrom(d=>d.CategoryId))
            .ForPath(dest=>dest.ParentId,opt=>opt.MapFrom(d=>d.ParentId))
            .ForPath(dest=>dest.Name,opt=>opt.MapFrom(d=>d.CategoryName));

        CreateMap<ProductRating, ProductRatingDto>();
        CreateMap<Promotion,PromotionDto>();
        CreateMap<Attribute,AttributeDto>();
        CreateMap<AddAttributeCommandV1.AddAttributeCommand,Attribute>();
        //CreateMap<CompanyProductSaleEntry, CompanyProductSaleEntryDto>();
        // CreateMap<ProductDto,Product>().ForPath(dest=>dest.CategoryId,opt=>opt.MapFrom(p=>p.CategoryId));

        CreateMap<AddProductCommandV1.AddProductCommand, Product>();
        
        CreateMap<Product,ProductDto>();


    }
}