using System.Collections.Generic;
using POC.BL.Domain.setup;

namespace POC.BL.Domain.user
{
    public class Admin : User
    {
        public ICollection<SetUpAdmin> SetUps { get; set; }
    }
}