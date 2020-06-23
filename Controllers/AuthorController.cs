using System.Threading.Tasks;
using BookAPI.Entities;
using BookAPI.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AuthorController : ControllerBase
    {
        private IAuthor _author;
        public AuthorController(IAuthor author)
            {
                _author = author;
            }

        [HttpPost]
        public void Post([FromBody] Author author)
        {
            _author.Add(author);
        }

        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor([FromBody] Author author)
        {
            var createAuthor = await _author.AddAsync(author);

            if (createAuthor)
            {
                return Ok("Author Created");
            }
            else{
                return BadRequest(new { message = "Unable to create Author details" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _author.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _author.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Author author)
        {
            author.Id = id;
            var updateAuthor = await _author.Update(author);

            if (updateAuthor)
            {
                return Ok("Author Updated");
            }
            else
            {
                return BadRequest(new { message = "Unable to update Author details" });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteAuthor = await _author.Delete(id);
            if (deleteAuthor)
            {
                return Ok("Author Deleted");
            }
            else
            {
                return BadRequest(new { message = "Unable to delete Author details" });
            }
        }
    }
}
