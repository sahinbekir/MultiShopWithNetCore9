using AutoMapper;
using MongoDB.Driver;
using MultiShopWithNetCore9.Catalog.Dtos.ProductImageDtos;
using MultiShopWithNetCore9.Catalog.Entities;
using MultiShopWithNetCore9.Catalog.Settings;

namespace MultiShopWithNetCore9.Catalog.Services.ProductImageServices;

public class ProductImageService : IProductImageService
{
    private readonly IMongoCollection<ProductImage> _catalogCollection;
    private readonly IMapper _mapper;
    public ProductImageService(IMapper mapper, IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);
        _catalogCollection = database.GetCollection<ProductImage>(_databaseSettings.ProductImageCollectionName);
        _mapper = mapper;
    }
    public async Task CreateProductImageAsync(CreateProductImageDto createProductImageDto)
    {
        var value = _mapper.Map<ProductImage>(createProductImageDto);
        await _catalogCollection.InsertOneAsync(value);
    }

    public async Task DeleteProductImageAsync(string id)
    {
        await _catalogCollection.DeleteOneAsync(x => x.ProductImageId == id);
    }

    public async Task<List<ResultProductImageDto>> GetAllProductImageAsync()
    {
        var values = await _catalogCollection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultProductImageDto>>(values);
    }

    public async Task<GetByIdProductImageDto> GetByIdProductImageAsync(string id)
    {
        var value = await _catalogCollection.Find<ProductImage>(x => x.ProductImageId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductImageDto>(value);
    }

    public async Task UpdateProductImageAsync(UpdateProductImageDto updateProductImageDto)
    {
        var value = _mapper.Map<ProductImage>(updateProductImageDto);
        await _catalogCollection.FindOneAndReplaceAsync(x => x.ProductImageId == updateProductImageDto.ProductImageId, value);
    }
}
