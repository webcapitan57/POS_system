using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.profile;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class ProfileRepository : IProfileRepository
    {
        private PhotoAppDbContext _ctx = null;
        
        public ProfileRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public StudentProfileQuestion ReadStudentProfileQuestion(int id)
        {
            return _ctx.StudentProfileQuestions
                .FirstOrDefault(question => question.ProfileQuestionId == id);
        }

        public StudentMCProfileQuestion ReadMcStudentProfileQuestion(int id)
        {
            return _ctx.StudentMcProfileQuestions
                .Include(q => q.StudentProfileOptions)
                .FirstOrDefault(q => q.ProfileQuestionId == id);
        }

        public void AddStudentProfileAnswers(List<StudentProfileAnswer> studentProfileAnswers)
        {
            _ctx.StudentProfileAnswers.AddRange(studentProfileAnswers);
            _ctx.SaveChanges();
        }
    }
}