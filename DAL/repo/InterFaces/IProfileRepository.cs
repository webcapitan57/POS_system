using POC.BL.Domain.profile;

namespace POC.DAL.repo.InterFaces
{
    public interface IProfileRepository
    {
        public StudentProfileQuestion ReadStudentProfileQuestion(int id);
        public StudentMCProfileQuestion ReadMcStudentProfileQuestion(int id);
    }
}