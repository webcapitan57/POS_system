using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class SetUpDTO
    {
        [Required]
        public string Name { get; set; }
        public string GeneralText { get; set; }
        
        public bool CreateTasks { get; set; }

        public string PrimColor { get; set; }
        public string SecColor { get; set; }
        public bool Archived { get; set; }
        public bool AllowLocations { get; set; }
    }
}