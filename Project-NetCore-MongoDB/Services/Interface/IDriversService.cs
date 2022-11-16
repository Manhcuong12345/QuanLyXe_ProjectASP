using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Models;

namespace Project_NetCore_MongoDB.Services.Interface
{
    public interface IDriversService
    {
        Task<List<Drivers>> GetAllAsync();
        Task<Drivers> GetByIdAsync(string id);
        Task<Drivers> CreateAsync(DriversDto drivers);
        Task UpdateAsync(string id, DriversDto drivers);
        Task DeleteAsync(string id);
    }
}
