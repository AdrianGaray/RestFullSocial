using Microsoft.EntityFrameworkCore;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFulSocial.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DB_SocialMediaContext _context;
        public UserRepository(DB_SocialMediaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }
    }
}
