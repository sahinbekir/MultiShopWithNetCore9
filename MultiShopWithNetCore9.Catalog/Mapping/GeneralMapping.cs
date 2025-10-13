using AutoMapper;
using MultiShopWithNetCore9.Catalog.Dtos.CategoryDtos;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDetailDtos;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDtos;
using MultiShopWithNetCore9.Catalog.Dtos.ProductImageDtos;
using MultiShopWithNetCore9.Catalog.Entities;

namespace MultiShopWithNetCore9.Catalog.Mapping;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
        CreateMap<Category, ResultCategoryDto>().ReverseMap();
        CreateMap<Category, GetByIdCategoryDto>().ReverseMap();
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();

        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, ResultProductDto>().ReverseMap();
        CreateMap<Product, GetByIdProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();

        CreateMap<ProductDetail, CreateProductDetailDto>().ReverseMap();
        CreateMap<ProductDetail, ResultProductDetailDto>().ReverseMap();
        CreateMap<ProductDetail, GetByIdProductDetailDto>().ReverseMap();
        CreateMap<ProductDetail, UpdateProductDetailDto>().ReverseMap();

        CreateMap<ProductImage, CreateProductImageDto>().ReverseMap();
        CreateMap<ProductImage, ResultProductImageDto>().ReverseMap();
        CreateMap<ProductImage, GetByIdProductImageDto>().ReverseMap();
        CreateMap<ProductImage, UpdateProductImageDto>().ReverseMap();
    }
}
