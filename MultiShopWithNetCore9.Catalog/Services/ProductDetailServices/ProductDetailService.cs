using AutoMapper;
using MongoDB.Driver;
using MultiShopWithNetCore9.Catalog.Dtos.ProductDetailDtos;
using MultiShopWithNetCore9.Catalog.Entities;
using MultiShopWithNetCore9.Catalog.Settings;

namespace MultiShopWithNetCore9.Catalog.Services.ProductDetailServices;

public class ProductDetailService : IProductDetailService
{
    private readonly IMongoCollection<ProductDetail> _catalogCollection;
    private readonly IMapper _mapper;
    public ProductDetailService(IMapper mapper, IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);
        _catalogCollection = database.GetCollection<ProductDetail>(_databaseSettings.ProductDetailCollectionName);
        _mapper = mapper;
    }
    public async Task CreateProductDetailAsync(CreateProductDetailDto createProductDetailDto)
    {
        var value = _mapper.Map<ProductDetail>(createProductDetailDto);
        await _catalogCollection.InsertOneAsync(value);
    }

    public async Task DeleteProductDetailAsync(string id)
    {
        await _catalogCollection.DeleteOneAsync(x => x.ProductDetailId == id);
    }

    public async Task<List<ResultProductDetailDto>> GetAllProductDetailAsync()
    {
        var values = await _catalogCollection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultProductDetailDto>>(values);
    }

    public async Task<GetByIdProductDetailDto> GetByIdProductDetailAsync(string id)
    {
        var value = await _catalogCollection.Find<ProductDetail>(x => x.ProductDetailId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdProductDetailDto>(value);
    }

    public async Task UpdateProductDetailAsync(UpdateProductDetailDto updateProductDetailDto)
    {
        var value = _mapper.Map<ProductDetail>(updateProductDetailDto);
        await _catalogCollection.FindOneAndReplaceAsync(x => x.ProductDetailId == updateProductDetailDto.ProductDetailId, value);
    }
}
