using RESTFulSocial.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFulSocial.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetUsers();
    }
}