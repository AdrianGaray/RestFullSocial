using RESTFulSocial.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTFulSocial.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        void UpDate(T entity);
        Task Delete(int id);

    }
}
