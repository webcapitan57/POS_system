using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class GroupRepository : IGroupRepository
    {
        private PhotoAppDbContext _ctx;

        public GroupRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Group ReadGroupByCode(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .FirstOrDefault(g => g.GroupCode == groupCode);
        }

        public void AddGroup(Group @group)
        {
            throw new System.NotImplementedException();
        }

        public Group ReadGroupWithTeacherByCode(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(g => g.Teacher)
                .ThenInclude(s => s.SetUp)
                .ThenInclude(s => s.NeededProfileQuestions)
                .Include(g => g.Info)
                .FirstOrDefault(g => g.GroupCode == groupCode);
        }

        public Group ReadGroupForShowActionByCode(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Info)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s=>s.NeededProfileQuestions)
                .ThenInclude(q=>(q as StudentMCProfileQuestion).StudentProfileOptions)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.UserAccount)
                .Include(g => g.GroupProfileAnswers)
                .Include(g => g.Tasks)
                .ThenInclude(gt => gt.Task)
                .ThenInclude(task => task.Questions)
                .ThenInclude(pq => pq.SideQuestions)
                .ThenInclude(sq => sq.SideQuestionOptions)
                .FirstOrDefault(g => g.GroupCode.Equals(groupCode));
        }

        public Group ReadGroupWithTeacherAndTasksByCode(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Tasks)
                .ThenInclude(gt => gt.Task)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .FirstOrDefault(g => g.GroupCode == groupCode);
        }

        public Group ReadGroupWithDeliveries(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.TaskDeliveries)
                .ThenInclude(d => d.Answers)
                .ThenInclude(a => a.AssignedPhoto)
                .SingleOrDefault(g => g.GroupCode == groupCode);
        }

        public void CreateGroup(Group group)
        {
            _ctx.Groups.Add(group);
            _ctx.SaveChanges();
        }

        public GroupInfo ReadGroupInfoOfGroup(string groupCode)
        {
            return _ctx.GroupInfos.FirstOrDefault(i => i.Group.GroupCode == groupCode);
        }

        public void UpdateGroupInfo(GroupInfo info)
        {
            _ctx.GroupInfos.Update(info);
            _ctx.SaveChanges();
        }

        public string GetGroupCode(string code)
        {
            return _ctx.Groups.FirstOrDefault(g => g.GroupCode == code)?.GroupCode;
        }

        public void DeleteAnswers(string groupCode)
        {
            IEnumerable<Answer> answersToDelete = ReadFlaggedAnswersOfGroup(groupCode);
            _ctx.Answers.RemoveRange(answersToDelete);
            _ctx.SaveChanges();
        }

        


        private IEnumerable<Answer> ReadFlaggedAnswersOfGroup(string groupCode)
        {
            return _ctx.Answers.Where(a => a.TaskDelivery.Group.GroupCode.Equals(groupCode) && a.Flagged);
        }

        public void UpdateGroup(Group group)
        {
            _ctx.Groups.Update(group);
            _ctx.SaveChanges();
        }

        public void RemoveGroup(string groupcode)
        {
            Group groupToDelete = ReadGroupByCode(groupcode);
            _ctx.Groups.Remove(groupToDelete);
            _ctx.SaveChanges();
        }

        public int NumberOfGroups()
        {
            return _ctx.Groups.Count();
        }

        public void ClearGroups()
        {
            foreach (var group in _ctx.Groups)
            {
                // RemoveGroup(group);
            }
        }

        public IEnumerable<Group> ReadGroupsOfTeacher(int teacherId)
        {
            IEnumerable<Group> teacherGroups = _ctx.Groups
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(g => g.Tasks)
                .Include(g => g.Teacher)
                .Where(g => g.Teacher.UserId == teacherId);
            return teacherGroups;
        }

        public IEnumerable<Group> ReadGroupsOfTeacher(string id)
        {
            IEnumerable<Group> teacherGroups = _ctx.Groups
                .Include(g => g.Tasks)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Where(g => g.Teacher.UserAccount.Id == id);
            return teacherGroups;
        }

        public Group ReadGroupWithTasksByCode(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(g => g.Tasks)
                .ThenInclude(t => t.Task)
                .Include(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .FirstOrDefault(g => g.GroupCode == groupCode);
        }

        public Group ReadGroupWithTasksAndAnswers(string groupCode)
        {
            return _ctx.Groups
                .Include(g => g.Tasks)
                .ThenInclude(t => t.Task)
                .Include(g=>g.GroupProfileAnswers)
                .ThenInclude(gpa=>gpa.AnsweredQuestion)
                .FirstOrDefault(g => g.GroupCode == groupCode);
        }

        public IEnumerable<GroupProfileQuestion> ReadGroupProfileQuestionsOfSetUp(int SetUpId)
        {
            return _ctx.GroupProfileQuestions.Where(q => q.SetUp.SetUpId == SetUpId)
                .Include(q => q.SetUp)
                .ThenInclude(s => s.Logo);
        }

        public IEnumerable<GroupMCProfileQuestion> ReadGroupMcProfileQuestions(int setupId)
        {
            return _ctx.GroupMcProfileQuestions
                .Include(q => q.GroupProfileOptions)
                .Where(q => q.SetUp.SetUpId == setupId);
        }

        public void CreateGroupProfileAnswer(GroupProfileAnswer groupProfileAnswer)
        {
            UpdateGroup(groupProfileAnswer.Group);
            _ctx.GroupProfileAnswers.Add(groupProfileAnswer);
            _ctx.SaveChanges();
        }
    }
}