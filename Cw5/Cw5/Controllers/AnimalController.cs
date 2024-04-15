using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cw5.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDb _Db;
        public AnimalController(IConfiguration configuration, IDb db)
        {
            _Db = db;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> GetAnimals()
        {
            var animals = await _Db.GetAnimalList();
            return Ok(animals);
        }


        [HttpPost]
        public async Task<IActionResult> AddAnimal(Animal animal)
        {
            if (await _Db.Exist(animal.ID))
            {
                return Conflict();
            }

            await _Db.CreateAnimal(new Animal
            {
                ID = animal.ID,
                Weight = animal.Weight,
                Name = animal.Name,
                Colour = animal.Colour,
                Category = animal.Category,
            });
            return Created($"/api/animals/{animal.ID}", animal);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnimal(string id, Animal animal)
        {
            if (await _Db.UpdateAnimal(id, animal))
            {
                return Ok(animal);
            }
            return NotFound();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnimal(string id)
        {
            if (await _Db.DeleteAnimal(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}