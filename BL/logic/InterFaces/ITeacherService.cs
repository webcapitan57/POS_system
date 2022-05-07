using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface ITeacherService
    {
        public Teacher GetTeacher(int id);
        public Teacher GetTeacher(string id);
        public Teacher GetTeacherWithGroups(string id);
        public Teacher GetTeacherWithGroupsAndTasks(string id);
        public IEnumerable<Teacher> GetAllTeachers();
        public void RemoveTeacher(Teacher teacher);
        public Teacher CreateTeacher(int id);
        public void UpdateTeacher(Teacher teacher);
        public void AddQualityScoreEvent(QualityScoreEvent qualityScoreEvent);
        public void RemoveQualityScoreEvent(int teacherId, int answerId);
        public IEnumerable<Teacher> GetAllTeachersWithUserAccount();
        public Teacher GetTeacherForSet(string userAccountId);
        
    }
}