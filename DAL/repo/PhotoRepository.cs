using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.delivery;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class PhotoRepository : IPhotoRepository
    {
        private PhotoAppDbContext _ctx ;
        
        public PhotoRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Photo AddPhoto(Photo photo)
        {

            _ctx.Photos.Add(photo);
            _ctx.SaveChanges();

            return photo;
        }

        public Photo ReadPhotoById(int id)
        {
            return _ctx.Photos.Find(id);
        }

        public int NumberOfPhotos()
        {
            return _ctx.Photos.Count();
        }

        public void RemovePhoto(Photo photo)
        {
            _ctx.Photos.Remove(photo);
            _ctx.SaveChanges();
        }

        public void RemovePhotos(IEnumerable<Photo> photos)
        {
            _ctx.Photos.RemoveRange(photos);
            _ctx.SaveChanges();
        }

        public Photo ReadSetupLogo(int setUpId)
        {
            return _ctx.Photos.FirstOrDefault(p => p.SetUp.SetUpId == setUpId);
        }

        public void UpdatePhoto(Photo logoToChange)
        {
            _ctx.Photos.Update(logoToChange);
            _ctx.SaveChanges();
        }
    }
}