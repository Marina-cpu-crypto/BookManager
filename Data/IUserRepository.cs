using WriterApp.Models;

namespace WriterApp.Data
{
    public interface IUserRepository
    {
        List<User> GetAll();
        void Save(Guid UserId);
    }
}