using System.Collections.Generic;
using POC.BL.Domain.delivery;

namespace POC.BL.Domain.user
{
    public class FilterProfile
    {
        public int FilterProfileId { get; set; }
        public string ProfileName { get; set; }
        public User User { get; set; }
        public IList<Filter> Filters { get; set; }
    }
}