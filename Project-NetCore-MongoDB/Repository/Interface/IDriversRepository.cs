using Project_NetCore_MongoDB.Models;

namespace Project_NetCore_MongoDB.Repository.Interface
{
    public interface IDriversRepository
    {
        Task<List<Drivers>> GetAllAsync();
        Task<Drivers> GetByIdAsync(string id);
        Task<Drivers> CreateAsync(Drivers drivers);
        Task UpdateAsync(string id, Drivers drivers);
        Task DeleteAsync(string id);
    }
}
