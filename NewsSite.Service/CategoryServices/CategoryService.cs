using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IUnitOfWork uow)
        {
            _categoryRepository = uow.GetRepository<Category>();
        }

        public IQueryable<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category Find(int id)
        {
            return _categoryRepository.Find(id);
        }

        public void Insert(Category category)
        {
            _categoryRepository.Insert(category);
        }

        public void Update(Category category)
        {
            _categoryRepository.Update(category);
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }
    }
}
