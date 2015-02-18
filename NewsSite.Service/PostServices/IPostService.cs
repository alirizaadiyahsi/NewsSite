using NewsSite.Core.Database.Tables;
using System.Linq;

namespace NewsSite.Service.PostServices
{
    public interface IPostService
    {
        IQueryable<Post> GetAll();

        Post Find(int id);

        void Insert(Post post);

        void Update(Post post);

        void Delete(Post post);

       IQueryable<PostPosition> GetAllPositions();
    }
}
