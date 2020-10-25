using RESTFulSocial.Core.Entities;
using RESTFulSocial.Core.Interfaces;
using RESTFulSocial.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RESTFulSocial.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DB_SocialMediaContext _contex;
        private readonly IPostRepository _postRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Comment> _commentRepository;
        public UnitOfWork(DB_SocialMediaContext context)
        {
            _contex = context;
        }

        public IPostRepository PostRepository => _postRepository ?? new PostRepository(_contex);

        public IRepository<User> UserRepository => _userRepository ?? new BaseRepository<User>(_contex);

        public IRepository<Comment> CommentRepository => _commentRepository ?? new BaseRepository<Comment>(_contex);

        public void Dispose()
        {
            if (_contex != null)
            {
                _contex.Dispose();
            }
        }

        public void SaveChanges()
        {
            _contex.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _contex.SaveChangesAsync();
        }
    }
}
