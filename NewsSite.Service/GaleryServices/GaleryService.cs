using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.PictureGaleryServices
{
    public class PictureGaleryService : IPictureGaleryService
    {
        private readonly IGenericRepository<PictureGalery> _PictureGaleryRepository;
        private readonly IGenericRepository<Picture> _PictureRepository;

        public PictureGaleryService(IUnitOfWork uow)
        {
            _PictureGaleryRepository = uow.GetRepository<PictureGalery>();
            _PictureRepository = uow.GetRepository<Picture>();
        }

        public IQueryable<PictureGalery> GetAll()
        {
            return _PictureGaleryRepository.GetAll();
        }

        public PictureGalery Find(int id)
        {
            return _PictureGaleryRepository.Find(id);
        }

        public void Insert(PictureGalery PictureGalery)
        {
            _PictureGaleryRepository.Insert(PictureGalery);
        }

        public void Update(PictureGalery PictureGalery)
        {
            _PictureGaleryRepository.Update(PictureGalery);
        }

        public void Delete(PictureGalery PictureGalery)
        {
            _PictureGaleryRepository.Delete(PictureGalery);
        }


        public IQueryable<Picture> GetPictures(int PictureGaleryId)
        {
            return _PictureRepository.GetAll().Where(x => x.PictureGaleryId == PictureGaleryId);
        }

        public void Insert(Picture Picture)
        {
            _PictureRepository.Insert(Picture);
        }

        public void Update(Picture Picture)
        {
            _PictureRepository.Update(Picture);
        }

        public void Delete(Picture Picture)
        {
            _PictureRepository.Delete(Picture);
        }

        public Picture FindPicture(int id)
        {
            return _PictureRepository.Find(id);
        }
    }
}
