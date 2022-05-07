using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class FilterProfileRepository : IFilterProfileRepository
    {
        private PhotoAppDbContext _ctx;

        public FilterProfileRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public FilterProfile ReadFilterProfile(int filterProfileId)
        {
            return _ctx.FilterProfiles
                .Include(p => p.Filters)
                .FirstOrDefault(p => p.FilterProfileId == filterProfileId);
        }

        public void AddFilterProfile(FilterProfile filterProfile)
        {
            _ctx.FilterProfiles.Add(filterProfile);
            _ctx.SaveChanges();
        }
    }
}