using Documents.Models;

namespace Documents.Data
{
    public interface ICollectionsRepository
    {
        List<Collection> GetOne(Guid id);
        //void Remove(Book book, int ind);
        void ResetCollection();
        void ResaveUserData(List<Collection> collections);
    }
}