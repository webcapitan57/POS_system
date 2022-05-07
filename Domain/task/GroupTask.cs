using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.user;

namespace POC.BL.Domain.task
{
    public class GroupTask
    {
        [Required]
        public Task Task { get; set; }
        [Required]
        public  Group Group { get; set; }
    }
}