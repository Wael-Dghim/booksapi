using BookStoreApi.Models;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) =>
        _booksService = booksService;

    [HttpGet]
    public async Task<List<Book>> GetBooks()
    {
        Console.Write("hh");
        return await _booksService.GetBooks();
    }
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Book>> GetOne(string id)
    {
        var book = await _booksService.GetBook(id);

        if (book is null) { return NotFound(); }

        return book;
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateOne(string id, Book updatedBook)
    {
        var book = await _booksService.GetBook(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateBook(id, updatedBook);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOne(Book newBook)
    {
        await _booksService.CreateBook(newBook);
        return CreatedAtAction(nameof(GetBooks), new { id = newBook.Id }, newBook);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteOne(string id)
    {
        var book = await _booksService.GetBook(id);

        if (book is null)
        {
            return NotFound();
        }

        await _booksService.DeleteBook(id);
        return NoContent();
    }
}