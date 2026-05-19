using Documents.Models;

namespace Documents.Data
{
    public interface IBookRepository
    {
        List<Book> GetAll();
        Book? TryGetById(Guid id);
    }
}