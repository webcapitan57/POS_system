using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.delivery;
using POC.BL.Domain.task;

namespace POC.BL.Domain.user
{
    public class Group
    {
        [Key] public int GroupId { get; set; }
        [Required] public string GroupCode { get; set; }
        public bool Active { get; set; }
        public bool AcceptDeliveries { get; set; } = true;
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        
        [Required] public GroupInfo Info { get; set; }
        public ICollection<GroupProfileAnswer> GroupProfileAnswers { get; set; }
        public ICollection<GroupTask> Tasks { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<TaskDelivery> TaskDeliveries { get; set; }
        public bool HasLimit { get; set; }
        
        [Range(1, short.MaxValue)]
        public short? MaxParticipants { get; set; }
    }
}