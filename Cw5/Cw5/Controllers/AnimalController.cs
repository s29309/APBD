using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cw5.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDb _db;
        public AnimalController(IConfiguration configuration, IDb db)
        {
            _db = db;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAnimals()
        {
            var animals = await _db.GetAnimalList();
            return Ok(animals);
        }


        [HttpPost]
        public async Task<IActionResult> AddAnimal(Animal animal)
        {
            if (await _db.Exist(animal.Id))
            {
                return Conflict();
            }

            await _db.CreateAnimal(new Animal
            {
                Id = animal.Id,
                Name = animal.Name,
                Description = animal.Description,
                Category = animal.Category,
				Area = animal.Area,
            });
            return Created($"/api/animals/{animal.Id}", animal);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnimal(string id, Animal animal)
        {
            if (await _db.UpdateAnimal(id, animal))
            {
                return Ok(animal);
            }
            return NotFound();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            if (await _db.DeleteAnimal(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}