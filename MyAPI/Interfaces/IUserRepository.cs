using MyAPI.Models;

namespace MyAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> Get(int page, int maxResultsPerPage);

        Task<User?> Get(string username, string password);

        Task<User> Insert(User user);
    }
}
