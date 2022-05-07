using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.delivery;

namespace POC.BL.Domain.task
{
    public class PhotoQuestion
    {
        public int PhotoQuestionId { get; set; }
        public string? Question { get; set; }
        public bool OnlyLocations { get; set; }
        public string? Tips { get; set; }
        
        public ICollection<SideQuestion> SideQuestions { get; set; }
        public ICollection<Location> Locations { get; set; }
        
        [Required]
        public Task Task { get; set; }
        public ICollection<Answer> Answers { get; set; }
        
    }
}