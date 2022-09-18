using MyAPI.Context;
using MyAPI.Interfaces;
using MyAPI.Models;

namespace MyAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InMemoryContext _context;

        public UserRepository(InMemoryContext context)
        {
            _context = context;
        }

        public Task<List<User>> Get(int page, int maxResultsPerPage)
        {
            return Task.Run(() =>
            {
                var user = _context.User.Skip((page - 1) * maxResultsPerPage).Take(maxResultsPerPage).ToList();

                return user.Any() ? user : new List<User>();
            });
        }

        public Task<User?> Get(string login, string password)
        {
            return Task.Run(() =>
            {
                var user = _context.User.FirstOrDefault(item =>
                    item.Login!.Equals(login, StringComparison.InvariantCultureIgnoreCase)
                    && item.Password!.Equals(password));

                return user;
            });
        }

        public Task<User> Insert(User user)
        {
            return Task.Run(() =>
            {
                _context.Add(user);
                _context.SaveChanges();

                return user;
            });
        }
    }
}