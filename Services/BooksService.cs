using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookStoreApi.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BooksService(
        IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings
    )
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
        _booksCollection = mongoDatabase.GetCollection<Book>(bookStoreDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<Book>> GetBooks() => await _booksCollection.Find(_ => true).ToListAsync();
    public async Task<Book?> GetBook(string Id) => await _booksCollection.Find(book => book.Id == Id).FirstOrDefaultAsync();
    public async Task CreateBook(Book newBook) => await _booksCollection.InsertOneAsync(newBook);
    public async Task UpdateBook(string Id, Book updatedBook) => await _booksCollection.ReplaceOneAsync(book => book.Id == Id, updatedBook);
    public async Task DeleteBook(string Id) => await _booksCollection.DeleteOneAsync(book => book.Id == Id);
}