using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Models;
using Project_NetCore_MongoDB.Services.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project_NetCore_MongoDB.Controllers
{
    [Route("api/drivers")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ICarsService _carsService;
        private readonly IDriversService _driversService;
        private readonly IMapper _mapper;
        // GET: api/<AuthsController>

        public DriversController(ICarsService carsService, IMapper mapper, IDriversService driversService)
        {
            _carsService = carsService;
            _mapper = mapper;
            _driversService = driversService;
        }
        // GET: api/<ArticlesController>
        //[Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _driversService.GetAllAsync());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            var drivers = await _driversService.GetByIdAsync(id);

            if (drivers == null)
            {
                return NotFound($"Drivers is not found!");
            }

            drivers.Cars = await _carsService.GetByIdAsync(drivers.CarId);

            return Ok(drivers);
        }

        // [Authorize(Policy = "UserPolicy")]
        [HttpPost]
        public async Task<IActionResult> Create(DriversDto drivers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            //lay _id tu token khi user do dang nhap
            //var userId = HttpContext.User.Claims.First(i => i.Type == "id").Value;

            //check kiem tra _id cua user va authorId khi nhap gia tri tao, neu khong khop tra ve loi
            // if (userId == null)
            // {
            //    return BadRequest(new { message = "Not authorized to create this articles" });
            // }

            // articles.AuthorId = userId;
            var driversData = await _driversService.CreateAsync(drivers);

            return CreatedAtAction(nameof(Get), new { id = driversData.Id }, driversData);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, DriversDto drivers)
        {
            var data = await _driversService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound($"Drivers is not found!");
            }

            //var userIdToken = HttpContext.User.Claims.First(i => i.Type == "id").Value;
            //check Id author in artiles vs id token login user. If worng then error. True continue
            //if (userIdToken != data.AuthorId)
            //{
            //return BadRequest(new { message = "Not authorized to update this articles" });
            // }

            await _driversService.UpdateAsync(id, drivers);

            return CreatedAtAction(nameof(Get), new { id = drivers.Id }, drivers);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var drivers = await _driversService.GetByIdAsync(id);

            if (drivers == null)
            {
                return NotFound($"Drivers is not found!");
            }

            await _driversService.DeleteAsync(id).ConfigureAwait(false);

            return Ok($"Drivers with Id = {id} is deleted");
        }
    }
}
