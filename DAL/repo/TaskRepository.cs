using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class TaskRepository : ITaskRepository
    {
        private PhotoAppDbContext _ctx;
        

        public TaskRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task ReadTaskById(int taskId)
        {
            var task = _ctx.Tasks
                .First(t => t.TaskId == taskId);
            switch (task)
            {
                case SetTask _:
                    return _ctx.SetTasks
                        .Include(st => st.SetUp)
                        .ThenInclude( s => s.Logo)
                        .Include(st => st.Questions)
                        .ThenInclude(pq => pq.SideQuestions)
                        .ThenInclude(sq => sq.SideQuestionOptions)
                        .Include(st => st.Questions)
                        .ThenInclude(q => q.Locations)
                        .First(t => t.TaskId == taskId);
                case TeacherTask _:
                    return _ctx.TeacherTasks
                        .Include(tt => tt.Teacher)
                        .ThenInclude(t => t.SetUp)
                        .ThenInclude(s => s.Logo)
                        .Include(tt => tt.Questions)
                        .ThenInclude(pq => pq.SideQuestions)
                        .ThenInclude(sq => sq.SideQuestionOptions)
                        .Include(st => st.Questions)
                        .ThenInclude(q => q.Locations)
                        .Include(tt => tt.Teacher)
                        .ThenInclude(t => t.SetUp)
                        .First(t => t.TaskId == taskId);
            }

            return null;
        }

        public PhotoQuestion ReadPhotoQuestionById(int id)
        {
            return _ctx.PhotoQuestions
                .Include(question => question.SideQuestions)
                .ThenInclude(sq => sq.SideQuestionOptions)
                .First(pq => pq.PhotoQuestionId == id);
        }

        public SideQuestion ReadSideQuestionById(int id)
        {
            return _ctx.SideQuestions
                .Include(sq => sq.SideQuestionOptions)
                .First(sq => sq.SideQuestionId == id);
        }

        public virtual SetTask AddSetTask(SetTask setTask)
        { 
            _ctx.SetUps.Update(setTask.SetUp);
           _ctx.SetTasks.Add(setTask);
           _ctx.SaveChanges();
           SetTask newSetTask = _ctx.SetUps
               .Include(s => s.SetTasks)
               .First(s => s.SetUpId == setTask.SetUp.SetUpId)
               .SetTasks.Last();
           return newSetTask;
        }

        public PhotoQuestion AddPhotoQuestion(PhotoQuestion photoQuestion)
        {
            _ctx.Tasks.Update(photoQuestion.Task);
            _ctx.PhotoQuestions.Add(photoQuestion);
            _ctx.SaveChanges();
            PhotoQuestion newPhotoQuestion = _ctx.Tasks
                .Include(task => task.Questions)
                .First(t => t.TaskId == photoQuestion.Task.TaskId)
                .Questions.Last();
            return newPhotoQuestion;
        }

        public SideQuestion AddSideQuestion(SideQuestion sideQuestion)
        {
            _ctx.PhotoQuestions.Update(sideQuestion.PhotoQuestion);
            _ctx.SideQuestions.Add(sideQuestion);
            _ctx.SaveChanges(); 
            SideQuestion newSideQuestion = _ctx.PhotoQuestions
                .Include(pq => pq.SideQuestions)
                .First(pq => pq.PhotoQuestionId == sideQuestion.PhotoQuestion.PhotoQuestionId)
                .SideQuestions.Last();
            return newSideQuestion;
        }

        public SideQuestionOption AddSideQuestionOption(SideQuestionOption sideQuestionOption)
        {
            _ctx.SideQuestions.Update(sideQuestionOption.MultipleChoiceSideQuestion);
            _ctx.SideQuestionOptions.Add(sideQuestionOption);
            _ctx.SaveChanges(); 
            SideQuestionOption newSideQuestionOption = _ctx.SideQuestions
                .Include(sq => sq.SideQuestionOptions)
                .First(sq => sq.SideQuestionId == sideQuestionOption.MultipleChoiceSideQuestion.SideQuestionId)
                .SideQuestionOptions.Last();
            return newSideQuestionOption;
        }
        public void UpdateTask(Task task)
        {
            _ctx.Tasks.Update(task);
            _ctx.SaveChanges();
        }

        public void RemoveTask(Task task)
        {
            _ctx.Tasks.Remove(task);
            _ctx.SaveChanges();
        }

        public void RemovePhotoQuestion(PhotoQuestion photoQuestion)
        {
            _ctx.PhotoQuestions.Remove(photoQuestion);
            _ctx.SaveChanges();
        }

        public void RemoveSideQuestion(SideQuestion sideQuestion)
        {
            _ctx.SideQuestions.Remove(sideQuestion);
            _ctx.SaveChanges();
        }

        public void ClearSideQuestionOption(SideQuestion sideQuestion)
        {
            var toDelete = _ctx.SideQuestionOptions.Where(sqo => sqo.MultipleChoiceSideQuestion == sideQuestion);
             _ctx.SideQuestionOptions.RemoveRange(toDelete);
             _ctx.SaveChanges();
        }

        public int NumberOfTasks()
        {
            return _ctx.Tasks.Count();
        }

        public void ClearTasks()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<SetTask> ReadSetTasksOfSetup(int setupId)
        {
            return _ctx.SetUps
                .Include(s => s.Logo)
                .Include(task => task.SetTasks)
                .ThenInclude(task => task.Questions)
                .ThenInclude(pq => pq.SideQuestions)
                .ThenInclude(sq => sq.SideQuestionOptions)
                .First(setUp => setUp.SetUpId == setupId ).SetTasks;

        }

        public IEnumerable<TeacherTask> ReadTeacherTasksOfTeacher(int teacherId)
        {
            return _ctx.TeacherTasks
                .Include(t => t.Teacher)
                .Include(t => t.Questions)
                .Where(t => t.Teacher.UserId == teacherId);
        }

        public TeacherTask AddTeacherTask(TeacherTask teacherTask)
        {
            _ctx.Teachers.Update(teacherTask.Teacher);
            _ctx.TeacherTasks.Add(teacherTask);
            _ctx.SaveChanges();
            TeacherTask newTeacherTask = _ctx.Teachers
                .Include(s => s.TeacherTasks)
                .First(s => s.UserId == teacherTask.Teacher.UserId)
                .TeacherTasks.Last();
            return newTeacherTask;
        }
        
    }
}