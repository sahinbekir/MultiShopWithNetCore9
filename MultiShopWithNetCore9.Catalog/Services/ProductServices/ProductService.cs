using AutoMapper;
using MongoDB.Driver;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDtos;
using MultiShopWithNetCore9.Catalog.Entities;
using MultiShopWithNetCore9.Catalog.Settings;

namespace MultiShopWithNetCore9.Catalog.Services.ProductServices;

public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _catalogCollection;
    private readonly IMapper _mapper;
    public ProductService(IMapper mapper, IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);
        _catalogCollection = database.GetCollection<Product>(_databaseSettings.ProductCollectionName);
        _mapper = mapper;
    }
    public async Task CreateProductAsync(CreateProductDto createProductDto)
    {
        var value = _mapper.Map<Product>(createProductDto);
        await _catalogCollection.InsertOneAsync(value);
    }

    public async Task DeleteProductAsync(string id)
    {
        await _catalogCollection.DeleteOneAsync(x => x.ProductId == id);
    }

    public async Task<List<ResultProductDto>> GetAllProductAsync()
    {
        var values = await _catalogCollection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultProductDto>>(values);
    }

    public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
    {
        var value = await _catalogCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductDto>(value);
    }

    public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
    {
        var value = _mapper.Map<Product>(updateProductDto);
        await _catalogCollection.FindOneAndReplaceAsync(x => x.ProductId == updateProductDto.ProductId, value);
    }
}
