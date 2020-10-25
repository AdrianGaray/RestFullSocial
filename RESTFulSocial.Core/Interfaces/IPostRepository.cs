using RESTFulSocial.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFulSocial.Core.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        // se manda el id del usuario
        Task<IEnumerable<Post>> GetPostsByUser(int userId);
    }
}
