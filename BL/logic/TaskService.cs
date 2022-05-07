using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISetupRepository _setupRepository;
        private readonly ILocationRepository _locationRepository;

        public TaskService(ITaskRepository taskRepository, ISetupRepository setupRepository, ILocationRepository locationRepository)
        {
            _taskRepository = taskRepository;
            _setupRepository = setupRepository;
            _locationRepository = locationRepository;
        }

        public Task ReadTaskById(int id)
        {
            return _taskRepository.ReadTaskById(id);
        }
        
        public IEnumerable<SetTask> GetSetTasksOfSetup(int setupId)
        {
            return _taskRepository.ReadSetTasksOfSetup(setupId);
        }

        public IEnumerable<TeacherTask> GetTeacherTasksOfTeacher(int teacherId)
        {
            return _taskRepository.ReadTeacherTasksOfTeacher(teacherId);
        }

        public Task GetTask(int taskId)
        {
            return _taskRepository.ReadTaskById(taskId);
        }

        public void AddLocation(Location location)
        {
            _locationRepository.AddLocation(location);
        }

        public void UpdateLocation(Location location)
        {
            _locationRepository.UpdateLocation(location);
        }

        public void DeleteLocation(int locationId)
        {
            var location = _locationRepository.ReadLocationById(locationId);
            _locationRepository.RemoveLocation(location);
        }

        public Location GetLocation(int locationId)
        {
            return _locationRepository.ReadLocationById(locationId);
        }

        public SetTask AddSetTask(SetUp setUp)
        {
            SetTask setTask = new SetTask
            {
                Questions = new List<PhotoQuestion>(),
                SetUp = setUp,
            };
            setTask.SetUp.SetTasks.Add(setTask);
            setTask.Questions.Add(new PhotoQuestion
            {
                Task = setTask,
                SideQuestions = new List<SideQuestion>()
            });
            SetTask newSetTask = _taskRepository.AddSetTask(setTask);
            return newSetTask;
        }

        public TeacherTask AddTeacherTask(Teacher teacher)
        {
            TeacherTask teacherTask = new TeacherTask()
            {
                Questions = new List<PhotoQuestion>(),
                Teacher = teacher,
            };
            teacherTask.Teacher.TeacherTasks.Add(teacherTask);
            teacherTask.Questions.Add(new PhotoQuestion
            {
                Task = teacherTask,
                SideQuestions = new List<SideQuestion>()
            });
            TeacherTask newtTeacherTask = _taskRepository.AddTeacherTask(teacherTask);
            return newtTeacherTask;
        }

        public void UpdateTask(Task task)
        {
            _taskRepository.UpdateTask(task);
        }

        public void DeleteTask(Task task)
        {
            _taskRepository.RemoveTask(task);
        }

        public void DeleteSideQuestionOption(int sideQuestionId)
        {
            SideQuestion sideQuestion = _taskRepository.ReadSideQuestionById(sideQuestionId);
            _taskRepository.ClearSideQuestionOption(sideQuestion);
        }

        public void CheckEmptyTasks(int setupId)
        {
            var tasks = _setupRepository.ReadSetupById(setupId).SetTasks;
            CheckTasks(new List<Task>(tasks));
        }

        public void CheckEmptyTasks(Teacher teacher)
        {
            var tasks = teacher.TeacherTasks;
            CheckTasks(new List<Task>(tasks));
        }

        private void CheckTasks(ICollection<Task> tasks)
        {
            for (int m = 0; m < tasks.Count; m++)
            {
                var setTask = tasks.ToList()[m];
                var task = _taskRepository.ReadTaskById(setTask.TaskId);
                if (string.IsNullOrWhiteSpace(task.Title))
                {
                    DeleteTask(task);
                    continue;
                }

                for (var i = 0; i < task.Questions.Count; i++)
                {
                    var question = task.Questions.ToList()[i];
                    if (string.IsNullOrWhiteSpace(question.Question))
                    {
                        DeletePhotoQuestion(question.PhotoQuestionId);
                        i--;
                        continue;
                    }

                    for (var j = 0; j < question.SideQuestions.Count; j++)
                    {
                        var sideQuestion = question.SideQuestions.ToList()[j];
                        for (var k = 0; k < sideQuestion.SideQuestionOptions.Count; k++)
                        {
                            var sideQuestionOption = sideQuestion.SideQuestionOptions.ToList()[k];
                            
                            if (!string.IsNullOrWhiteSpace(sideQuestionOption.Value)) continue;
                            
                            DeleteSideQuestionOption(sideQuestionOption.SideQuestionOptionId);
                            k--;
                        }

                        if (!string.IsNullOrWhiteSpace(sideQuestion.Question) &&
                            sideQuestion.SideQuestionOptions.Count != 1) continue;
                        
                        DeleteSideQuestion(sideQuestion.SideQuestionId);
                        j--;
                    }
                }
            }
        }

        public PhotoQuestion AddPhotoQuestion(Task task)
        {
            PhotoQuestion photoQuestion = new PhotoQuestion
            {
                Task = task
            };
            photoQuestion.Task.Questions.Add(photoQuestion);
            photoQuestion = _taskRepository.AddPhotoQuestion(photoQuestion);
            return photoQuestion;
        }

        public SideQuestion AddSideQuestion(PhotoQuestion photoQuestion)
        {
            SideQuestion sideQuestion = new SideQuestion
            {
                PhotoQuestion = photoQuestion
            };
            sideQuestion.PhotoQuestion.SideQuestions.Add(sideQuestion);
            sideQuestion = _taskRepository.AddSideQuestion(sideQuestion);
            return sideQuestion;
        }

        public void DeletePhotoQuestion(int questionId)
        {
            PhotoQuestion photoQuestion = _taskRepository.ReadPhotoQuestionById(questionId);
            if (photoQuestion != null)
            {
                _taskRepository.RemovePhotoQuestion(photoQuestion);
            }
        }

        public void DeleteSideQuestion(int sideQuestionId)
        {
            SideQuestion sideQuestion = _taskRepository.ReadSideQuestionById(sideQuestionId);
            if (sideQuestion != null)
            {
                _taskRepository.RemoveSideQuestion(sideQuestion);
            }
        }

        public SideQuestionOption AddSideQuestionOption(SideQuestion sideQuestion)
        {
            SideQuestionOption sideQuestionOption = new SideQuestionOption()
            {
                MultipleChoiceSideQuestion = sideQuestion
            };
            sideQuestionOption = _taskRepository.AddSideQuestionOption(sideQuestionOption);
            return sideQuestionOption;
        }
    }
}