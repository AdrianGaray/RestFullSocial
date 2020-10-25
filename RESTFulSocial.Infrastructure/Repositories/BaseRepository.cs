using Microsoft.EntityFrameworkCore;
using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTFulSocial.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {        
        private readonly DB_SocialMediaContext _context;
        // se cambia de private a protected para que sea visible, por la misma clase y aquellas que la heredan
        protected readonly DbSet<T> _entities;
        public BaseRepository(DB_SocialMediaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return  _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
           await _entities.AddAsync(entity);
        }

        public void UpDate(T entity)
        {
            _entities.Update(entity);            
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}
