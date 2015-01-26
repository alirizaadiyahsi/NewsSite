using NewsSite.Core.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.GaleryServices
{
   public interface IGaleryService
    {
       IQueryable<Galery> GetAll();
       Galery Find(int id);
       void Insert(Galery galery);
       void Update(Galery galery);
       void Delete(Galery galery);

       IQueryable<GaleryImage> GetGaleryImages(int galeryId);
       GaleryImage FindGaleryImage(int id);
       void Insert(GaleryImage galeryImage);
       void Update(GaleryImage galeryImage);
       void Delete(GaleryImage galeryImage);
    }
}
