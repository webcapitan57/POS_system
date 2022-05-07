using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using POC.BL.Domain.delivery;

namespace POC.BL.Domain.user
{
    public class Student
    {
        public int StudentId { get; set; }
        
        public ICollection<StudentProfileAnswer> ProfileAnswers { get; set; } 
        
        public Group Group { get; set; }

        public ICollection<TaskDelivery> Deliveries { get; set; }
    }
}