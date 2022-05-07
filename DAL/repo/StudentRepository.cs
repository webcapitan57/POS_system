using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class StudentRepository : IStudentRepository
    {
        private PhotoAppDbContext _ctx = null;


        public StudentRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Student ReadStudentById(int id)
        {
            return _ctx.Students
                .Include(s => s.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .FirstOrDefault(student => student.StudentId == id);

        }

        public void AddStudent(Student student)
        {
            _ctx.Students.Add(student);
            _ctx.Groups.Update(student.Group);
            _ctx.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            _ctx.Students.Update(student);
            _ctx.SaveChanges();
        }

        public void RemoveStudent(Student student)
        {
            _ctx.Students.Remove(student);
            _ctx.SaveChanges();
        }

        public int NumberOfStudents()
        {
            return _ctx.Students.Count();
        }

        public void ClearStudents()
        {
            throw new System.NotImplementedException();
        }
        
        public Student ReadStudentWithProfileAnswersById(int studentId)
        {
            return _ctx.Students
                .Include(s => s.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(student=>student.ProfileAnswers)
                .ThenInclude(pa => pa.AnsweredQuestion)
                .FirstOrDefault(student => student.StudentId == studentId);
        }
    }
}