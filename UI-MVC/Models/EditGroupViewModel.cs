using System.ComponentModel.DataAnnotations;

namespace UI_MVC.Models
{
    public class EditGroupViewModel
    {

        public string groupCode { get; set; }
        [Required(ErrorMessage = "Gelieve de groep een naam te geven.")]
        [StringLength(50)]
        public string GroupName { get; set; }
        [Range(1, short.MaxValue, ErrorMessage = "Gelieve een getal groter dan 0 in te geven.")]
        public short? MaxParticipants { get; set; }

        public EditGroupTaskViewModel EditGroupTaskViewModel { get; set; }
    }
}