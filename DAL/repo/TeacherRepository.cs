using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class TeacherRepository : ITeacherRepository
    {
        private PhotoAppDbContext _ctx;
        
        public TeacherRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public Teacher ReadTeacherById(int id)
        {
           return _ctx.Teachers.Find(id);
        }
        
        public Teacher ReadTeacherById(string id)
        {
            return _ctx.Teachers
                .Include(t=>t.SetUp)
                .Include(t=>t.UserAccount)
                .First(t=>t.UserAccountId.Equals(id));
        }

        public IEnumerable<Teacher> ReadAllTeachers()
        {
            return _ctx.Teachers;
        }

        public void AddTeacher(Teacher teacher)
        { 
            _ctx.Teachers.Add(teacher);
            _ctx.SaveChanges();
        }

        public void UpdateTeacher(Teacher teacher)
        {
            _ctx.Teachers.Update(teacher);
            _ctx.SaveChanges();
        }

        public void RemoveTeacher(Teacher teacher)
        {
            var account = _ctx.Users.Find(teacher.UserAccountId);
            _ctx.Teachers.Remove(teacher);
            _ctx.Users.Remove(account);
            _ctx.SaveChanges();
        }

        public int NumberOfTeachers()
        {
            return _ctx.Teachers.Count();
        }

        public Teacher ReadTeacherWithGroupsById(string id)
        {
            return _ctx.Teachers
                .Include(t=>t.SetUp)
                .Include(t=>t.UserAccount)
                .Include(t=>t.Groups)
                .First(t=>t.UserAccountId.Equals(id));
        }

        public Teacher ReadTeacherWithGroupsAndTaskById(string id)
        {
            return _ctx.Teachers
                .Include(t=>t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(t=>t.UserAccount)
                .Include(t=>t.Groups)
                .ThenInclude(g=>g.Tasks)
                .Include(t=>t.TeacherTasks)
                .ThenInclude(tt=>tt.Questions)
                .First(t=>t.UserAccountId.Equals(id));
        }

        public Teacher ReadTeacherWithEvents(int id)
        {
            return _ctx.Teachers
                .Include(t => t.QualityScoreEvents)
                .ThenInclude(e => e.Answer)
                .FirstOrDefault(t => t.UserId == id);
        }

        public void AddQualityScoreEvent(QualityScoreEvent qualityScoreEvent)
        {
            _ctx.QualityScoreEvents.Add(qualityScoreEvent);
            _ctx.SaveChanges();
        }

        public void RemoveQualityScoreEvent(int teacherId, int answerId)
        {
            var ev = _ctx.QualityScoreEvents.FirstOrDefault(e => e.Answer.AnswerId == answerId && e.Teacher.UserId == teacherId);
            _ctx.QualityScoreEvents.Remove(ev);
            _ctx.SaveChanges();
        }

        public Teacher ReadTeacherForSet(int teacherId)
        {
            return _ctx.Teachers
                .Include(t => t.QualityScoreEvents)
                .ThenInclude(e => e.Answer)
                .Include(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(t => t.FilterProfiles)
                .FirstOrDefault(t => t.UserId == teacherId);
            
        }
        public IEnumerable<Teacher> ReadAllTeachersWithUserAccount()
        {
            return _ctx.Teachers
                    .Include(t => t.UserAccount);
        }

        public Teacher ReadTeacherForSet(string userAccountId)
        {
            return _ctx.Teachers
                .Include(t => t.QualityScoreEvents)
                .ThenInclude(e => e.Answer)
                .Include(t => t.SetUp)
                .Include(t => t.FilterProfiles)
                .FirstOrDefault(t => t.UserAccountId == userAccountId);

        }
    }
}