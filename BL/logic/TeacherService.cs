using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISetupService _setupService;

        public TeacherService(ITeacherRepository teacherRepository, ISetupService setupService)
        {
            _teacherRepository = teacherRepository;
            _setupService = setupService;
        }

        public Teacher GetTeacher(int id)
        {
            return _teacherRepository.ReadTeacherById(id);
        }
        
        public Teacher GetTeacher(string id)
        {
            return _teacherRepository.ReadTeacherById(id);
        }

        public Teacher GetTeacherWithGroups(string id)
        {
            return _teacherRepository.ReadTeacherWithGroupsById(id);
        }

        public Teacher GetTeacherWithGroupsAndTasks(string id)
        {
            return _teacherRepository.ReadTeacherWithGroupsAndTaskById(id);
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _teacherRepository.ReadAllTeachers();
        }

        public void RemoveTeacher(Teacher teacher)
        {
           _teacherRepository.RemoveTeacher(teacher);
        }

        public Teacher CreateTeacher(int id)
        {
            var setup = _setupService.ReadSetupById(id);
            return new Teacher()
            {
                IsGuest = true,
                SetUp = setup
            };
        }

        public void UpdateTeacher(Teacher teacher)
        {
            _teacherRepository.UpdateTeacher(teacher);
        }

        public void AddQualityScoreEvent(QualityScoreEvent qualityScoreEvent)
        {
            _teacherRepository.AddQualityScoreEvent(qualityScoreEvent);
        }

        public void RemoveQualityScoreEvent(int teacherId, int answerId)
        {
            _teacherRepository.RemoveQualityScoreEvent(teacherId, answerId);
        }

        public Teacher GetTeacherForSet(string teacherId)
        {
            return _teacherRepository.ReadTeacherForSet(teacherId);
        }

        public IEnumerable<Teacher> GetAllTeachersWithUserAccount()
        {
            return _teacherRepository.ReadAllTeachersWithUserAccount();
        }
    }
}