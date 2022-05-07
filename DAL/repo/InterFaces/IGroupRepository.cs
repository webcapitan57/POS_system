using System.Collections.Generic;
using POC.BL.Domain.profile;
using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface IGroupRepository
    {
        public Group ReadGroupByCode(string groupCode);
        public void UpdateGroup(Group group);
        public void RemoveGroup(string groupCode);
        public Group ReadGroupWithTeacherByCode(string groupCode);
        public Group ReadGroupForShowActionByCode(string groupCode);
        public IEnumerable<Group> ReadGroupsOfTeacher(string teacherId);
        public Group ReadGroupWithTasksByCode(string groupCode);
        public Group ReadGroupWithTasksAndAnswers(string groupCode);
        public IEnumerable<GroupProfileQuestion> ReadGroupProfileQuestionsOfSetUp(int groupId);
        public IEnumerable<GroupMCProfileQuestion> ReadGroupMcProfileQuestions(int setupId);
        public Group ReadGroupWithTeacherAndTasksByCode(string groupCode);
        public void CreateGroup(Group group);
        public GroupInfo ReadGroupInfoOfGroup(string groupCode);
        public void UpdateGroupInfo(GroupInfo info);
        public string GetGroupCode(string code);
        public void DeleteAnswers(string groupCode);
    }
}