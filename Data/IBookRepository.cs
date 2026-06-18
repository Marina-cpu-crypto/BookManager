using WriterApp.Models;

namespace WriterApp.Data
{
    public interface IBookRepository
    {
        void Change(Book book);
        void ChangeStatus(Guid id);
        void Delete(Guid id);
        void Resave();
    }
}
