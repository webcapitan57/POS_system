using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.user;

namespace POC.BL.Domain.setup
{
    public class SetUpAdmin
    {
        [Required] 
        public SetUp SetUp { get; set; }
        
        [Required] 
        public Admin Admin { get; set; }
        
    }
}