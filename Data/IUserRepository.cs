using Documents.Models;

namespace Documents.Data
{
    public interface IUserRepository
    {
        List<User> GetAll();
        void Save(Guid UserId);
    }
}