using Documents.Models;
using Documents.Models;

namespace Documents.Data
{
    public interface IBookRepository
        {
            List<Book> GetAll();
            Book? TryGetById(Guid id);
            void Change(Book book);
            void ChangeStatus(Book book, bool status);
            void Delete(Book book);
            void Resave();
            //void Sort();
    }
}
