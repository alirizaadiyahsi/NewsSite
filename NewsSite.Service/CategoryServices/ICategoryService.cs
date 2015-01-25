using NewsSite.Core.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.CategoryServices
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAll();

        Category Find(int id);

        void Insert(Category category);

        void Update(Category category);

        void Delete(Category category);
    }
}
