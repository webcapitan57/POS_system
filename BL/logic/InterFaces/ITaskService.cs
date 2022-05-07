using System.Collections.Generic;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface ITaskService
    {
        public Task ReadTaskById(int id);
        public SetTask AddSetTask(SetUp setUp);
        public TeacherTask AddTeacherTask(Teacher teacher);
        public PhotoQuestion AddPhotoQuestion(Task task);
        public SideQuestion AddSideQuestion(PhotoQuestion photoQuestion);
        public SideQuestionOption AddSideQuestionOption(SideQuestion sideQuestion);
        public void UpdateTask(Task task);
        public void DeleteTask(Task task);
        public void DeletePhotoQuestion(int questionId);
        public void DeleteSideQuestion(int sideQuestionId);
        public void DeleteSideQuestionOption(int sideQuestionId);
        public void CheckEmptyTasks(int setupId);
        public void CheckEmptyTasks(Teacher teacher);
        public IEnumerable<SetTask> GetSetTasksOfSetup(int setupId);
        public IEnumerable<TeacherTask> GetTeacherTasksOfTeacher(int teacherId);
        public Task GetTask(int taskId);
        public void AddLocation(Location location);
        public void UpdateLocation(Location location);
        public void DeleteLocation(int locationId);
        public Location GetLocation(int locationId);
    }
}