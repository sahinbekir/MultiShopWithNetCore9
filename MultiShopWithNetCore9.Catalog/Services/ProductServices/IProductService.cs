using MultiShopWithNetCore9.Catalog.Dtos.ProductDtos;

namespace MultiShopWithNetCore9.Catalog.Services.ProductServices;

public interface IProductService
{
    Task<List<ResultProductDto>> GetAllProductAsync();
    Task CreateProductAsync(CreateProductDto createProductDto);
    Task UpdateProductAsync(UpdateProductDto updateProductDto);
    Task DeleteProductAsync(string id);
    Task<GetByIdProductDto> GetByIdProductAsync(string id);
}
