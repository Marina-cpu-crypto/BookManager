using Documents.Models;

namespace Documents.Data
{
    public interface ICollectionsRepository
    {
        List<Collection> GetAll();
        //Book? TryGetById(Guid id);
    }
}