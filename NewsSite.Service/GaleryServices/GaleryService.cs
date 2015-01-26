using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsSite.Service.GaleryServices
{
    public class GaleryService : IGaleryService
    {
        private readonly IGenericRepository<Galery> _galeryRepository;
        private readonly IGenericRepository<GaleryImage> _galeryImageRepository;

        public GaleryService(IUnitOfWork uow)
        {
            _galeryRepository = uow.GetRepository<Galery>();
            _galeryImageRepository = uow.GetRepository<GaleryImage>();
        }

        public IQueryable<Galery> GetAll()
        {
            return _galeryRepository.GetAll();
        }

        public Galery Find(int id)
        {
            return _galeryRepository.Find(id);
        }

        public void Insert(Galery galery)
        {
            _galeryRepository.Insert(galery);
        }

        public void Update(Galery galery)
        {
            _galeryRepository.Update(galery);
        }

        public void Delete(Galery galery)
        {
            _galeryRepository.Delete(galery);
        }


        public IQueryable<GaleryImage> GetGaleryImages(int galeryId)
        {
            return _galeryImageRepository.GetAll().Where(x => x.GaleryId == galeryId);
        }

        public void Insert(GaleryImage galeryImage)
        {
            _galeryImageRepository.Insert(galeryImage);
        }

        public void Update(GaleryImage galeryImage)
        {
            _galeryImageRepository.Update(galeryImage);
        }

        public void Delete(GaleryImage galeryImage)
        {
            _galeryImageRepository.Delete(galeryImage);
        }

        public GaleryImage FindGaleryImage(int id)
        {
            return _galeryImageRepository.Find(id);
        }
    }
}
