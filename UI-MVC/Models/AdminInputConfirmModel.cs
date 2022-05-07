using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.setup;

namespace UI_MVC.Models
{
    public class AdminInputConfirmModel
    {
        public string UserId { get; set; }
        
        [Required]
        [StringLength(30, ErrorMessage = "Het wachtwoord moet minstens {2} en maximum {1} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigingswachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}