using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudAPI.Data;
using CrudAPI.Models;

namespace CrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public BooksController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            if (_context.Books != null)
            {
                return await _context.Books.ToListAsync();
            }
            return NotFound("Book not found.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            if (_context.Books != null)
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null) return book;
            }
            return NotFound("Book not found.");
        }

        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            if (_context.Books != null)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            return Problem("Entity set is null.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (BookExists(id))
            {
                if (id == book.Id)
                {
                    _context.Entry(book).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok("Book updated successfully.");
                }
                else
                {
                    return BadRequest("Something went wrong.");
                }
            }
            return NotFound("Book not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books != null)
            {
                var book = await _context.Books.FindAsync(id);
                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    return Ok("Book deleted successully.");
                }
            }
            return NotFound("Book not found.");
        }

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
