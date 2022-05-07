using System.Collections.Generic;
using POC.BL.Domain.task;

namespace UI_MVC.Models
{
    public class PhotoQuestionDTO
    {
        public int PhotoQuestionId { get; set; }
        public string Question { get; set; }
        public string Tips { get; set; }
        public bool OnlyLocations { get; set; }
        public List<SideQuestionDTO> SideQuestions { get; set; }
        public List<LocationDTO> Locations { get; set; }
    }
}