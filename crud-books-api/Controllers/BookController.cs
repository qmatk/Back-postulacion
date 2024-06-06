using crud_books_api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace crud_books_api.Controllers
{
    [Authorize]
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public BookController() { }

        private List<Book> Populate()
        {
            List<Book> books = new List<Book>();
            books.Add(new Book()
            {
                Id = 1,
                Title = "Papelucho",
                Description = "Libro infantil",
                Author = "Gabriela Perez",
                Genre = "Infantil",
                PublicDate = DateTime.Now,
            }
            );

            books.Add(new Book()
            {
                Id = 2,
                Title = "Canción de hielo y fuego",
                Description = "Libro juvenil",
                Author = "George RR Martin",
                Genre = "Juvenil",
                PublicDate = DateTime.Now.AddMonths(-9),
            });

            return books;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetBooks()
        {
            List<Book> books = Populate();

            return Ok(books);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBooksById(int id)
        {
            var books = Populate();

            var found = books.Find(x => x.Id == id);

            if (found == null) return BadRequest(new { status = false, message = "Libro no encontrado" });
            
            return Ok(found);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {

            var books = Populate();

            var bookFound = books.Find(r => r.Id == id);
            if (bookFound == null) return BadRequest(new { message = "Libro no encontrado" });

            var bookRemoved = books.Remove(bookFound);

            if (!bookRemoved) return BadRequest(new { message = "No se pudo eliminar" });

            return Ok(new { message = "deleted " + id, books });
        }

        [HttpPost]
        [Route("new/")]
        public async Task<IActionResult> CreateBook([FromBody] Book request)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "No coindicen los tipos", ModelState });

            var books = Populate();

            if (books.Find(x => x.Id == request.Id) != null) return BadRequest(new { message = "Libro existente, favor ingrese otro ID" });

            books.Add(request);

            return Ok(new { message = "Libro agregado", books });
        }

        [HttpPost]
        [Route("update/")]
        public async Task<IActionResult> UpdateBook([FromBody] Book book)
        {
            var books = Populate();

            var bookFound = books.Find(x => x.Id == book.Id);

            if (bookFound == null) return BadRequest(new { status = false, message = "Libro no encontrado" });

            bookFound.Title = book.Title;
            bookFound.Description = book.Description;
            bookFound.Author = book.Author;
            bookFound.PublicDate = book.PublicDate;

            return Ok( new { message = "Libro actualizado", book = bookFound});
        }
    }
}
