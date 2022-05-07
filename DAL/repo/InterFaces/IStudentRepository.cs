using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface IStudentRepository
    {
        public Student ReadStudentById(int id);
        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void RemoveStudent(Student student);
        public Student ReadStudentWithProfileAnswersById(int studentId);
    }
}