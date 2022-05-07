using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.task;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class LocationRepository : ILocationRepository
    {
        private PhotoAppDbContext _ctx ;
        
        public LocationRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public Location ReadLocationById(int id)
        {
            return _ctx.Locations
                .Include(l => l.PhotoQuestion)
                .SingleOrDefault(l => l.LocationId == id);
        }

        public Location AddLocation(Location location)
        {
            _ctx.Locations.Add(location);
            _ctx.SaveChanges();
            return location;
        }

        public void UpdateLocation(Location location)
        {
            _ctx.Locations.Update(location);
            _ctx.SaveChanges();
        }

        public void RemoveLocation(Location location)
        {
            _ctx.Locations.Remove(location);
            _ctx.SaveChanges();
        }

        public int NumberOfLocations()
        {
            return _ctx.Locations.Count();
        }
    }
}