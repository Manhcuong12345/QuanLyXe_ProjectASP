using AutoMapper;
using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Repository.Interface;
using Project_NetCore_MongoDB.Services.Interface;

namespace Project_NetCore_MongoDB.Services
{
    public class CarsService : ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IMapper _mapper;

        public CarsService(ICarsRepository carsRepository, IMapper mapper)
        {
            _carsRepository = carsRepository;
            _mapper = mapper;
        }

        public Task<List<Cars>> GetAllAsync()
        {
            return _carsRepository.GetAllAsync();
        }

        public Task<Cars> GetByIdAsync(string id)
        {
            return _carsRepository.GetByIdAsync(id);
        }

        public Task<Cars> GetExistData(string license_plates)
        {
            return _carsRepository.GetExistData(license_plates);
        }

        public Task<Cars> CreateAsync(CarsDto cars)
        {
            var dataMapper = _mapper.Map<Cars>(cars);
            return _carsRepository.CreateAsync(dataMapper);
        }

        public Task UpdateAsync(string id, CarsDto cars)
        {
            var dataMapper = _mapper.Map<Cars>(cars);

            return _carsRepository.UpdateAsync(id, dataMapper);
        }

        public Task DeleteAsync(string id)
        {
            return _carsRepository.DeleteAsync(id);
        }

       
    }
}
