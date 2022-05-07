using POC.BL.Domain.task;

namespace POC.DAL.repo.InterFaces
{
    public interface ILocationRepository
    {
        public Location ReadLocationById(int id);
        public Location AddLocation(Location location);
        public void UpdateLocation(Location location);
        public void RemoveLocation(Location location);
    }
}