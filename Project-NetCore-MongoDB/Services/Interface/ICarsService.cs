using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Models;

namespace Project_NetCore_MongoDB.Services.Interface
{
    public interface ICarsService
    {
        Task<List<Cars>> GetAllAsync();
        Task<Cars> GetByIdAsync(string id);
        Task<Cars> CreateAsync(CarsDto cars);
        Task UpdateAsync(string id, CarsDto cars);
        Task DeleteAsync(string id);
        Task<Cars> GetExistData(string license_plates);
    }
}
