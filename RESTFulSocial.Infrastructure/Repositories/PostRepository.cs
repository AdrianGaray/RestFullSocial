using Microsoft.EntityFrameworkCore;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFulSocial.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(DB_SocialMediaContext context) : base(context) { }

        public async Task<IEnumerable<Post>> GetPostsByUser(int userId)
        {
            return await _entities.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
