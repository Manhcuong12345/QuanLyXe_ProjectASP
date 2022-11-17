using Project_NetCore_MongoDB.Models;

namespace Project_NetCore_MongoDB.Repository.Interface
{
    public interface ICarsRepository
    {
        Task<List<Cars>> GetAllAsync();
        Task<Cars> GetByIdAsync(string id);
        Task<Cars> CreateAsync(Cars cars);
        Task UpdateAsync(string id, Cars cars);
        Task DeleteAsync(string id);
        Task<Cars> GetExistData(string license_plates);
    }
}
