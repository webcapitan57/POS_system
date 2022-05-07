using System.Collections.Generic;

namespace POC.BL.Domain.profile
{
    public class StudentMCProfileQuestion : StudentProfileQuestion
    {
        public ICollection<StudentProfileOption> StudentProfileOptions { get; set; }
    }
}