using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface IStudentService
    {
        public Student AddStudent(Group group);
        public Student GetStudentWithTags(int studentId);
        public Student GetStudentWithProfileAnswers(int studentId);
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student);
        public Student GetStudent(int studentId);
    }
}