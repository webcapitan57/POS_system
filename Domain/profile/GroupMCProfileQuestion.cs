using System.Collections.Generic;

namespace POC.BL.Domain.profile
{
    public class GroupMCProfileQuestion : GroupProfileQuestion
    {
        public IList<GroupProfileOption> GroupProfileOptions { get; set; }
    }
}