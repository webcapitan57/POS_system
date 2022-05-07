using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface IGroupService
    {
        public Group GetGroup(string groupCode);
        public Group GetGroupWithTeacherAndTasks(string groupCode);
        public Group GetGroupWithTasksAndAnswers(string groupCode);
        public IEnumerable<StudentProfileQuestion> GetOpenProfileQuestions(Student student, Group group, List<KeyValuePair<string, string>> keyValuePairs);
        public IEnumerable<Group> GetTeacherGroups(string teacherId);
        public Group AddGroup(int setUpId, string name, Teacher teacher, string groupCode, short? maxParticipants = 0);
        public void AddGroupTask(GroupTask groupTask);
        public void RemoveGroupTask(GroupTask groupTask);
        public GroupTask GetGroupTask(int taskId, int groupId);
        public void RemoveGroup(string groupCode);
        public Group GetGroupWithTasks(string groupCode);
        public void ChangeGroup(Group group);
        public IEnumerable<GroupProfileQuestion> GetGroupProfileQuestions(int setUpId);
        public Group GetGroupWithTeacher(string groupCode);
        public StudentProfileQuestion GetStudentProfileQuestion(int id);
        public Group GetGroupForShowAction(string groupCode);
        public void UpdateTotalPhotosOfGroupInfo(Group group, TaskDelivery taskDeliveries);
        public void UpdateTotalStudentsOfGroupInfo(Group group);
        public bool GroupsCodeUnique(string code);
        public void RemoveAnswers(string groupCode);
        public void RemoveAnswer(int answerId);
        public void UpdateGroup(Group group);
    }
}