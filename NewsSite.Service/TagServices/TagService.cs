using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.TagServices
{
    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _tagRepository;

        public TagService(IUnitOfWork uow)
        {
            _tagRepository = uow.GetRepository<Tag>();
        }

        public IQueryable<Tag> GetAll()
        {
            return _tagRepository.GetAll();
        }

        public Tag Find(int id)
        {
            return _tagRepository.Find(id);
        }

        public void Insert(Tag tag)
        {
            _tagRepository.Insert(tag);
        }

        public void Update(Tag tag)
        {
            _tagRepository.Update(tag);
        }

        public void Delete(Tag tag)
        {
            _tagRepository.Delete(tag);
        }
    }
}
