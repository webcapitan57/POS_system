using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class CreateGroupViewModel : IValidatableObject
    {
        #region BasicInfo

        public int SetUpId { get; set; }
       // public string setUpName { get; set; }
        
        [Required(ErrorMessage = "Gelieve de groep een naam te geven.")]
        [StringLength(50)]
        public string GroupName { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Gelieve een getal groter dan 0 in te geven.")]
        public short? MaxParticipants { get; set; }

        #endregion

        #region profileAnswers

        public IList<GroupProfileAnswer> Answers { get; set; }
        public IList<ValidationResult> Errors { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (Answers == null) return errors;
            
            errors = (from profileAnswer in Answers
                    where profileAnswer.AnsweredQuestion.IsRequired && profileAnswer.Value == null
                    select new ValidationResult(profileAnswer.AnsweredQuestion.Description +
                                                " is verplicht in te vullen"))
                .ToList();
                
            //Errors = errors;
            return errors;
        }

        #endregion
        
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public SetUp Setup { get; set; }
    }
}