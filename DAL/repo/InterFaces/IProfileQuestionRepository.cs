using POC.BL.Domain.profile;

namespace POC.DAL.repo.InterFaces
{
    public interface IProfileQuestionRepository
    {
        public ProfileQuestion ReadProfileQuestionById(int id);
        public void RemoveProfileQuestion(ProfileQuestion profileQuestion);
    }
}