using System;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.profile;

namespace POC.BL.Domain.user
{
    public class GroupProfileAnswer
    {
        public int GroupProfileAnswerId { get; set; }
        public string Value { get; set; }
        
        [Required]
        public GroupProfileQuestion AnsweredQuestion{ get; set; }
        public Group Group { get; set; }
    }
}