using NewsSite.Core.Database.Tables;
using System.Linq;

namespace NewsSite.Service.TagServices
{
    public interface ITagService
    {
        IQueryable<Tag> GetAll();

        Tag Find(int id);

        void Insert(Tag tag);

        void Update(Tag tag);

        void Delete(Tag tag);
    }
}
