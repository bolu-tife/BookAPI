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
    public class BookController : ControllerBase
    {
        private IBook _book;
        public BookController(IBook book)
        {
            _book = book;
        }

        [HttpPost]
        public void Post([FromBody] Book book)
        {
            _book.Add(book);
        }

        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var createBook = await _book.AddAsync(book);

            if (createBook)
            {
                return Ok("Book Created");
            }
            else
            {
                return BadRequest(new { message = "Unable to create Book details" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _book.GetAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _book.GetById(id);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book book)
        {
            book.Id = id;
            var updateBook = await _book.Update(book);

            if (updateBook)
            {
                return Ok("Book Updated");
            }
            else
            {
                return BadRequest(new { message = "Unable to update Book details" });
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteBook = await _book.Delete(id);
            if (deleteBook)
            {
                return Ok("Book Deleted");
            }
            else
            {
                return BadRequest(new { message = "Unable to delete Book details" });
            }
        }
    }
}
