using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace POC.BL.Domain.profile
{
    public class StudentProfileQuestion : ProfileQuestion
    {
        
        public ICollection<StudentProfileAnswer> StudentProfileAnswers { get; set; }
    }
}