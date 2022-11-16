using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_NetCore_MongoDB.Dto;
using Project_NetCore_MongoDB.Services.Interface;
using Project_NetCore_MongoDB.Services;

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
        // GET: api/<ArticlesController>
        //[Authorize(Policy = "UserPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _carsService.GetAllAsync());
        }

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
        [HttpPost]
        public async Task<IActionResult> Create(CarsDto cars)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest($"Name is required");
            }

            var carsData = await _carsService.CreateAsync(cars);
            return CreatedAtAction(nameof(Get), new { id = carsData.Id }, carsData);

           


            //lay _id tu token khi user do dang nhap
            //var userId = HttpContext.User.Claims.First(i => i.Type == "id").Value;

            //check kiem tra _id cua user va authorId khi nhap gia tri tao, neu khong khop tra ve loi
            // if (userId == null)
            // {
            //    return BadRequest(new { message = "Not authorized to create this articles" });
            // }

            // articles.AuthorId = userId;
            //var carsData = await _carsService.CreateAsync(cars);

            //return CreatedAtAction(nameof(Get), new { id = carsData.Id }, carsData);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, CarsDto cars)
        {
            var data = await _carsService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound($"Cars is not found!");
            }

            //var userIdToken = HttpContext.User.Claims.First(i => i.Type == "id").Value;
            //check Id author in artiles vs id token login user. If worng then error. True continue
            //if (userIdToken != data.AuthorId)
            //{
                //return BadRequest(new { message = "Not authorized to update this articles" });
           // }

            await _carsService.UpdateAsync(id, cars);

            return CreatedAtAction(nameof(Get), new { id = cars.Id }, cars);
        }

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
