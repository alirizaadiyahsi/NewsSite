using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System.Linq;

namespace NewsSite.Service.PostServices
{
    public class PostService : IPostService
    {

        private readonly IGenericRepository<Post> _postRepository;
        private readonly IGenericRepository<PostPosition> _postPositionRepository;

        public PostService(IUnitOfWork uow)
        {
            _postRepository = uow.GetRepository<Post>();
            _postPositionRepository = uow.GetRepository<PostPosition>();
        }
        public IQueryable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }

        public Post Find(int id)
        {
            return _postRepository.Find(id);
        }

        public void Insert(Post post)
        {
            _postRepository.Insert(post);
        }

        public void Update(Post post)
        {
            _postRepository.Update(post);
        }

        public void Delete(Post post)
        {
            _postRepository.Delete(post);
        }

        public IQueryable<PostPosition> GetAllPositions()
        {
            return _postPositionRepository.GetAll();
        }
    }
}
