using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class GroupTaskRepository : IGroupTaskRepository
    {
        private PhotoAppDbContext _ctx;

        public GroupTaskRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public void CreateGroupTask(GroupTask groupTask)
        {
            _ctx.GroupTasks.Add(groupTask);
            _ctx.Groups.Update(groupTask.Group);
            _ctx.Tasks.Update(groupTask.Task);
            _ctx.SaveChanges();
        }

        public void DeleteGroupTask(GroupTask groupTask)
        {
            _ctx.GroupTasks.Remove(groupTask);
            _ctx.Groups.Update(groupTask.Group);
            _ctx.Tasks.Update(groupTask.Task);
            _ctx.SaveChanges();
        }

        public GroupTask ReadGroupTask(int taskId, int groupId)
        {
            return _ctx.GroupTasks
                .Include(gt => gt.Group)
                .Include(gt => gt.Task)
                .FirstOrDefault(gt => gt.Group.GroupId == groupId && gt.Task.TaskId == taskId);
        }
    }
}