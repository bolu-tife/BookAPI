using System.Threading.Tasks;
using BookAPI.Entities;
using BookAPI.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenreController : ControllerBase
    {
        private IGenre _genre;
        public GenreController(IGenre genre)
        {
            _genre = genre;
        }

        [HttpPost]
        public void Post([FromBody] Genre genre)
        {
            _genre.Add(genre);
        }

        [HttpPost("AddGenre")]
        public async Task<IActionResult> AddGenre([FromBody] Genre genre)
        {
            var createGenre = await _genre.AddAsync(genre);

            if (createGenre)
            {
                return Ok("Genre Created");
            }
            else
            {
                return BadRequest(new { message = "Unable to create Genre details" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genre.GetAll();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genre.GetById(id);
            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Genre genre)
        {
            genre.Id = id;
            var updateGenre = await _genre.Update(genre);

            if (updateGenre)
            {
                return Ok("Genre Updated");
            }
            else
            {
                return BadRequest(new { message = "Unable to update Genre details" });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteGenre = await _genre.Delete(id);
            if (deleteGenre)
            {
                return Ok("Genre Deleted");
            }
            else
            {
                return BadRequest(new { message = "Unable to delete Genre details" });
            }
        }
    }
}
