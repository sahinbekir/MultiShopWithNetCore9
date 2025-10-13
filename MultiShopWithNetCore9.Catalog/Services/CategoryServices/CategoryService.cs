using AutoMapper;
using MongoDB.Driver;
using MultiShopWithNetCore9.Catalog.Dtos.CategoryDtos;
using MultiShopWithNetCore9.Catalog.Entities;
using MultiShopWithNetCore9.Catalog.Settings;

namespace MultiShopWithNetCore9.Catalog.Services.CategoryServices;

public class CategoryService : ICategoryService
{

    private readonly IMongoCollection<Category> _catalogCollection;
    private readonly IMapper _mapper;
    public CategoryService(IMapper mapper, IDatabaseSettings _databaseSettings)
    {
        var client = new MongoClient(_databaseSettings.ConnectionString);
        var database = client.GetDatabase(_databaseSettings.DatabaseName);
        _catalogCollection = database.GetCollection<Category>(_databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var value = _mapper.Map<Category>(createCategoryDto);
        await _catalogCollection.InsertOneAsync(value);
    }

    public async Task DeleteCategoryAsync(string id)
    {
        await _catalogCollection.DeleteOneAsync(x => x.CategoryId == id);
    }

    public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
    {
        var values = await _catalogCollection.Find(x => true).ToListAsync();
        return _mapper.Map<List<ResultCategoryDto>>(values);
    }

    public async Task<GetByIdCategoryDto> GetByIdCategoryAsync(string id)
    {
        var value = await _catalogCollection.Find<Category>(x => x.CategoryId == id).FirstOrDefaultAsync();
        return _mapper.Map<GetByIdCategoryDto>(value);
    }

    public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
    {
        var value = _mapper.Map<Category>(updateCategoryDto);
        await _catalogCollection.FindOneAndReplaceAsync(x => x.CategoryId == updateCategoryDto.CategoryId, value);
    }
}
