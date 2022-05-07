using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.Domain.setup
{
    public class SetUp
    {
        [Key] public int SetUpId { get; set; }

        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Archived { get; set; }
        public string GeneralText { get; set; }
        public bool AllowLocations { get; set; }
        public ICollection<SetUpAdmin> Admins { get; set; }

        public ICollection<ProfileQuestion> NeededProfileQuestions { get; set; }

        public ICollection<SetTask> SetTasks { get; set; }
        public ICollection<GroupInfo> Usage { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public bool CreateTasks { get; set; }
        public string PrimColor { get; set; }
        public string SecColor { get; set; }
        
        public string loginIndentifier { get; set; }
       
        public Photo? Logo { get; set; }
    }
}