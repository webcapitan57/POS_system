using System.Collections.Generic;
using POC.BL.Domain.delivery;

namespace POC.DAL.repo.InterFaces
{
    public interface IPhotoRepository
    {
        public Photo AddPhoto(Photo photo);
        public void RemovePhoto(Photo photo);
        public void RemovePhotos(IEnumerable<Photo> toDeletePhotos);
        public Photo ReadSetupLogo(int setUpId);
        public void UpdatePhoto(Photo logoToChange);
    }
}