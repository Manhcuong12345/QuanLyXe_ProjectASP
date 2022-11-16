using AutoMapper;
using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Repository.Interface;
using Project_NetCore_MongoDB.Services.Interface;

namespace Project_NetCore_MongoDB.Services
{
    public class DriversService : IDriversService
    {
        private readonly IDriversRepository _driversRepository;
        private readonly IMapper _mapper;

        public DriversService(IDriversRepository driversRepository, IMapper mapper)
        {
            _driversRepository = driversRepository;
            _mapper = mapper;
        }

        public Task<List<Drivers>> GetAllAsync()
        {
            return _driversRepository.GetAllAsync();
        }

        public Task<Drivers> GetByIdAsync(string id)
        {
            return _driversRepository.GetByIdAsync(id);
        }

        public Task<Drivers> CreateAsync(DriversDto drivers)
        {
            var dataMapper = _mapper.Map<Drivers>(drivers);
            return _driversRepository.CreateAsync(dataMapper);
        }

        public Task UpdateAsync(string id, DriversDto drivers)
        {
            var dataMapper = _mapper.Map<Drivers>(drivers);

            return _driversRepository.UpdateAsync(id, dataMapper);
        }

        public Task DeleteAsync(string id)
        {
            return _driversRepository.DeleteAsync(id);
        }
    }
}
