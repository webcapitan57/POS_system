using System.Collections.Generic;
using POC.BL.Domain.task;

namespace POC.DAL.repo.InterFaces
{
    public interface ITaskRepository
    {
        public Task ReadTaskById(int id);
        public PhotoQuestion ReadPhotoQuestionById(int id);
        public SideQuestion ReadSideQuestionById(int id);
        public SetTask AddSetTask(SetTask setTask);
        public PhotoQuestion AddPhotoQuestion(PhotoQuestion photoQuestion);
        public SideQuestion AddSideQuestion(SideQuestion sideQuestion);
        public SideQuestionOption AddSideQuestionOption(SideQuestionOption sideQuestionOption);
        public void UpdateTask(Task task);      
        public void RemoveTask(Task task);
        public void RemovePhotoQuestion(PhotoQuestion photoQuestion);
        public void RemoveSideQuestion(SideQuestion sideQuestion);
        public void ClearSideQuestionOption(SideQuestion sideQuestion);
        public IEnumerable<SetTask> ReadSetTasksOfSetup(int setupId);
        public IEnumerable<TeacherTask> ReadTeacherTasksOfTeacher(int teacherId);
        public TeacherTask AddTeacherTask(TeacherTask teacherTask);
    }
}