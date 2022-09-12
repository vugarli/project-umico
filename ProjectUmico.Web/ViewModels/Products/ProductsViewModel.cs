using ProjectUmico.Application.Common;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Web.Models.Products;

public class ProductsViewModel
{
    public PaginatedList<ProductDto> Products { get; set; }
}