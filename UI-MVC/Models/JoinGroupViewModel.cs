using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.user;
using POC.BL.logic;

namespace UI_MVC.Models
{
    public class JoinGroupViewModel
    {
        [Required(ErrorMessage = "Geef een groepscode in")]
        public string GroupCode { get; set; }

        public IList<ValidationException> ValidationExceptions { get; set; }
    }
}