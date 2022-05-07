using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class ProfileViewModel : IValidatableObject
    {
        public IList<StudentProfileAnswer> Answers { get; set; }
        public IList<ValidationResult> Errors { get; set; }
        public Student Student { get; set; }
        public Group Group { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Errors = (from profileAnswer in Answers
                    where profileAnswer.AnsweredQuestion.IsRequired && profileAnswer.Value == null
                    select new ValidationResult(profileAnswer.AnsweredQuestion.Description +
                                                " is verplicht in te vullen"))
                .ToList();
            return Errors;
        }
    }
}