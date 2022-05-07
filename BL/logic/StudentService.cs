using System.Collections.Generic;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Student AddStudent(Group group)
        {
            var student = new Student()
            {
                Group = group,
                ProfileAnswers = new List<StudentProfileAnswer>()
                
            };

            _studentRepository.AddStudent(student);
            return student;
        }

        public Student GetStudentWithProfileAnswers(int studentId)
        {
            return _studentRepository.ReadStudentWithProfileAnswersById(studentId);
        }

        public Student GetStudentWithTags(int studentId)
        {
            return _studentRepository.ReadStudentWithProfileAnswersById(studentId);
        }

        public void UpdateStudent(Student student)
        {
            _studentRepository.UpdateStudent(student);
        }

        public void DeleteStudent(Student student)
        {
            _studentRepository.RemoveStudent(student);
        }
        
        public Student GetStudent(int studentId)
        {
            return _studentRepository.ReadStudentById(studentId);
        }
    }
}