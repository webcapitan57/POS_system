using POC.BL.Domain.task;

namespace POC.DAL.repo.InterFaces
{
    public interface IGroupTaskRepository
    {
        public void CreateGroupTask(GroupTask groupTask);
        public void DeleteGroupTask(GroupTask groupTask);
        public GroupTask ReadGroupTask(int taskId, int groupId);
    }
}