using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Repository.Interface;

namespace Project_NetCore_MongoDB.Repository
{
    public class DriversRepository : IDriversRepository
    {
        private readonly IMongoCollection<Drivers> _collection;
        private readonly DbConfiguration _settings;
        public DriversRepository(IOptions<DbConfiguration> settings)
        {
            _settings = settings.Value;
            var client = new MongoClient(_settings.ConnectionString);
            var database = client.GetDatabase(_settings.DatabaseName);
            _collection = database.GetCollection<Drivers>("drivers");
        }

        public Task<List<Drivers>> GetAllAsync()
        {
            return _collection.Find(c => true).ToListAsync();
        }
        public Task<Drivers> GetByIdAsync(string id)
        {
            return _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Drivers> CreateAsync(Drivers drivers)
        {
            await _collection.InsertOneAsync(drivers).ConfigureAwait(false);
            return drivers;
        }
        public Task UpdateAsync(string id, Drivers drivers)
        {
            return _collection.ReplaceOneAsync(c => c.Id == id, drivers, new UpdateOptions { IsUpsert = true });
        }
        public Task DeleteAsync(string id)
        {
            return _collection.DeleteOneAsync(c => c.Id == id);
        }
    }
}
