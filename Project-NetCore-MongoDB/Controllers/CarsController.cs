using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Services.Interface;
using Project_NetCore_MongoDB.Services;
using Project_NetCore_MongoDB.Middleware;
using Project_NetCore_MongoDB.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_NetCore_MongoDB.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarsService _carsService;
        private readonly IMapper _mapper;
        // GET: api/<AuthsController>

        public CarsController(ICarsService carsService, IMapper mapper)
        {
            _carsService = carsService;
            _mapper = mapper;
        }
        //tenantId
        // GET: api/<ArticlesController>
        //[Authorize(Policy = "UserPolicy")]

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userIdToken = HttpContext.User.Claims.First(i => i.Type == "jti").Value;
            if (userIdToken == null)
             {
                return BadRequest(new {message = "Not authorized to create this articles"});
             }
                return Ok(await _carsService.GetAllAsync());
        }

        [Authorize]
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var cars = await _carsService.GetByIdAsync(id);

            if (cars == null)
            {
                return NotFound($"Cars is not found!");
            }

            return Ok(cars);
        }

        // [Authorize(Policy = "UserPolicy")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CarsDto cars)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existData = await _carsService.GetExistData(cars.License_Plates);
            if (existData != null)
            {
                return BadRequest($"License_Plates is Exist");
            }

            var carsData = await _carsService.CreateAsync(cars);
           
            
            return CreatedAtAction(nameof(Get), new { id = carsData.Id }, carsData);

        }

        [Authorize]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, CarsDto cars)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var data = await _carsService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound($"Cars is not found!");
            }

            await _carsService.UpdateAsync(id, cars);

            return CreatedAtAction(nameof(Get), new { id = cars.Id }, cars);
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var cars = await _carsService.GetByIdAsync(id);

            if (cars == null)
            {
                return NotFound($"Cars is not found!");
            }

            await _carsService.DeleteAsync(id).ConfigureAwait(false);

            return Ok($"Cars with Id = {id} is deleted");
        }

    }
}
