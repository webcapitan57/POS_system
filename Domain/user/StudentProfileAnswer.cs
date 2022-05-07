using System;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.profile;

namespace POC.BL.Domain.user
{
    public class StudentProfileAnswer
    {
        public int StudentProfileAnswerId { get; set; }
        public string Value { get; set; }
        public Student Student { get; set; }
        [Required]
        public StudentProfileQuestion AnsweredQuestion{ get; set; }
        
    }
}