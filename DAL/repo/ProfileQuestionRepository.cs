using System.Linq;
using POC.BL.Domain.profile;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    //repo voor GroupProfileQuestion en StudentProfileQuestion
    public class ProfileQuestionRepository : IProfileQuestionRepository
    {
        private PhotoAppDbContext _ctx;
        
        public ProfileQuestionRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public ProfileQuestion ReadProfileQuestionById(int id)
        {
            return _ctx.ProfileQuestions.Find(id);
        }

        public void AddProfileQuestion(ProfileQuestion profileQuestion)
        {
            _ctx.SetUps.Update(profileQuestion.SetUp);
            _ctx.ProfileQuestions.Add(profileQuestion);
            _ctx.SaveChanges();
        }

        public void UpdateProfileQuestion(ProfileQuestion profileQuestion)
        {
            _ctx.ProfileQuestions.Update(profileQuestion);
            _ctx.SaveChanges();
        }

        public void RemoveProfileQuestion(ProfileQuestion profileQuestion)
        {
            _ctx.ProfileQuestions.Remove(profileQuestion);
            _ctx.SaveChanges();
        }

        public int NumberOfProfileQuestions()
        {
            return _ctx.ProfileQuestions.Count();
        }

        public void ClearProfileQuestions()
        {
            throw new System.NotImplementedException();
        }
    }
}