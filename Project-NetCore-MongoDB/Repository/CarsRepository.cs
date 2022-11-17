using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Repository.Interface;
using Project_NetCore_MongoDB.Dto;

namespace Project_NetCore_MongoDB.Repository
{
    public class CarsRepository : ICarsRepository
    {
        private readonly IMongoCollection<Cars> _collection;
        private readonly DbConfiguration _settings;
        public CarsRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Cars>("cars");
        }

        public Task<List<Cars>> GetAllAsync()
        {
            return _collection.Find(c => true).ToListAsync();
        }
        public Task<Cars> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public Task<Cars> GetExistData(string license_plates)
        {
            return _collection.Find(c => c.License_Plates == license_plates).FirstOrDefaultAsync();
        }

        public async Task<Cars> CreateAsync(Cars cars)
        {
            await _collection.InsertOneAsync(cars).ConfigureAwait(false);
            return cars;
        }
        public Task UpdateAsync(string id, Cars cars)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, cars, new UpdateOptions { IsUpsert = true });
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }
    }
}

