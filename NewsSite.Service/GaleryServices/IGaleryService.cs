using NewsSite.Core.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.PictureGaleryServices
{
   public interface IPictureGaleryService
    {
       IQueryable<PictureGalery> GetAll();
       PictureGalery Find(int id);
       void Insert(PictureGalery PictureGalery);
       void Update(PictureGalery PictureGalery);
       void Delete(PictureGalery PictureGalery);

       IQueryable<Picture> GetPictures(int PictureGaleryId);
       Picture FindPicture(int id);
       void Insert(Picture Picture);
       void Update(Picture Picture);
       void Delete(Picture Picture);
    }
}
