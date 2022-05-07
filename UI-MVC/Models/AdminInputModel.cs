using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.setup;

namespace UI_MVC.Models
{
    public class AdminInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public ICollection<SetUp> SetUps { get; set; }
        public int UserId { get; set; }
    }
}