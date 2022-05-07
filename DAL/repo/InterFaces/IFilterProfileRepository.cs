using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface IFilterProfileRepository
    {
        public FilterProfile ReadFilterProfile(int filterProfileId);
        public void AddFilterProfile(FilterProfile filterProfile);
    }
}