using WriterApp.Models;

namespace WriterApp.Data
{
    public interface ICollectionsRepository
    {
        List<UserData> GetAll();
        List<Collection> GetOne(Guid id);
        Book TryGetById(Guid bookid);
        //List<Book> GetBooks(Guid MainId);

        //void Remove(Book book, int ind);
        void ResetCollection();
        void ResaveUserData(List<Collection> collections);
    }
}
