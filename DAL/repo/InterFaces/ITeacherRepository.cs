using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface ITeacherRepository
    {
        public Teacher ReadTeacherById(int id);
        public Teacher ReadTeacherById(string id);
        public  IEnumerable<Teacher> ReadAllTeachers();
        public void UpdateTeacher(Teacher teacher);
        public void RemoveTeacher(Teacher teacher);
        public Teacher ReadTeacherWithGroupsById(string id);
        public Teacher ReadTeacherWithGroupsAndTaskById(string id);
        public void AddQualityScoreEvent(QualityScoreEvent qualityScoreEvent);
        public void RemoveQualityScoreEvent(int teacherId, int answerId);
        public IEnumerable<Teacher> ReadAllTeachersWithUserAccount();
        public Teacher ReadTeacherForSet(string userAccountId);
    }
}