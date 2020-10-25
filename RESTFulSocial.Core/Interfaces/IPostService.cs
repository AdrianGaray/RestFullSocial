using RESTFulSocial.Core.CustomEntities;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFulSocial.Core.Interfaces
{
    public interface IPostService
    {
        PagedList<Post> GetPosts(PostQueryFilter filters);
        Task<Post> GetPost(int id);
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(int id);
    }
}