using System;
using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupTaskRepository _groupTaskRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly ISetupRepository _setupRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public GroupService(IGroupRepository groupRepository, IProfileRepository profileRepository,
            IGroupTaskRepository groupTaskRepository, ISetupRepository setupRepository,
            IDeliveryRepository deliveryRepository)
        {
            _groupRepository = groupRepository;
            _groupTaskRepository = groupTaskRepository;
            _profileRepository = profileRepository;
            _setupRepository = setupRepository;
            _deliveryRepository = deliveryRepository;
        }

        public Group GetGroup(string groupCode)
        {
            var group = _groupRepository.ReadGroupByCode(groupCode);
            return group;
        }

        public Group GetGroupWithTeacherAndTasks(string groupCode)
        {
            return _groupRepository.ReadGroupWithTeacherAndTasksByCode(groupCode);
        }

        public Group GetGroupWithTasksAndAnswers(string groupCode)
        {
            return _groupRepository.ReadGroupWithTasksAndAnswers(groupCode);
        }

        public IEnumerable<StudentProfileQuestion> GetOpenProfileQuestions(Student student, Group group,
            List<KeyValuePair<string, string>> cookies)
        {
            var setUp = group.Teacher.SetUp;

            foreach (var profileQuestion in setUp.NeededProfileQuestions.OfType<StudentProfileQuestion>())
            {
                var (_, value) = cookies.FirstOrDefault(o => o.Key == profileQuestion.ProfileQuestionId.ToString());
                if (value == null) continue;

                student.ProfileAnswers.Add(new StudentProfileAnswer()
                {
                    AnsweredQuestion = profileQuestion,
                    Value = value,
                    Student = student
                });
            }

            var answeredQuestions =
                student.ProfileAnswers.Select(answer => answer.AnsweredQuestion).Distinct().ToList();

            var questions = setUp.NeededProfileQuestions.OfType<StudentProfileQuestion>()
                .Where(question => !answeredQuestions.Contains(question)).ToList();

            questions.AddRange(
                from answer in student.ProfileAnswers
                where answer.AnsweredQuestion.IsRequired && answer.Value == ""
                select answer.AnsweredQuestion);

            return questions.Select(profileQuestion => profileQuestion is StudentMCProfileQuestion
                    ? _profileRepository.ReadMcStudentProfileQuestion(profileQuestion.ProfileQuestionId)
                    : profileQuestion)
                .ToList();
        }

        public Group GetGroupWithTeacher(string groupCode)
        {
            return _groupRepository.ReadGroupWithTeacherByCode(groupCode);
        }

        public StudentProfileQuestion GetStudentProfileQuestion(int id)
        {
            var question = _profileRepository.ReadStudentProfileQuestion(id);
            return question is StudentMCProfileQuestion
                ? _profileRepository.ReadMcStudentProfileQuestion(id)
                : question;
        }

        public Group GetGroupForShowAction(string groupCode)
        {
            return _groupRepository.ReadGroupForShowActionByCode(groupCode);
        }

        private GroupInfo GetGroupInfo(string groupCode)
        {
            return _groupRepository.ReadGroupInfoOfGroup(groupCode);
        }

        public void UpdateTotalPhotosOfGroupInfo(Group @group, TaskDelivery delivery)
        {
            var info = GetGroupInfo(group.GroupCode);

            info.TotalPhotos += delivery.SentPhotos.Count;
            _groupRepository.UpdateGroupInfo(info);
        }

        public void UpdateTotalStudentsOfGroupInfo(Group @group)
        {
            var info = GetGroupInfo(group.GroupCode);

            info.TotalStudents += 1;
            _groupRepository.UpdateGroupInfo(info);
        }

        public bool GroupsCodeUnique(string code)
        {
            return _groupRepository.GetGroupCode(code) == null;
        }

        public void RemoveAnswers(string groupCode)
        {
            _groupRepository.DeleteAnswers(groupCode);
        }

        public void RemoveAnswer(int answerId)
        {
            _deliveryRepository.DeleteAnswer(answerId);
        }

        public void UpdateGroup(Group group)
        {
            _groupRepository.UpdateGroup(group);
        }

        public IEnumerable<Group> GetTeacherGroups(string teacherId)
        {
            return _groupRepository.ReadGroupsOfTeacher(teacherId);
        }
        
        public void AddGroupTask(GroupTask groupTask)
        {
            _groupTaskRepository.CreateGroupTask(groupTask);
        }

        public void RemoveGroupTask(GroupTask groupTask)
        {
            _groupTaskRepository.DeleteGroupTask(groupTask);
        }

        public GroupTask GetGroupTask(int taskId, int groupId)
        {
            return _groupTaskRepository.ReadGroupTask(taskId, groupId);
        }

        public void RemoveGroup(string groupCode)
        {
            _groupRepository.RemoveGroup(groupCode);
        }

        public Group GetGroupWithTasks(string groupCode)
        {
            return _groupRepository.ReadGroupWithTasksByCode(groupCode);
        }

        public void ChangeGroup(Group group)
        {
            _groupRepository.UpdateGroup(group);
        }

        public IEnumerable<GroupProfileQuestion> GetGroupProfileQuestions(int setUpId)
        {
            var tempList = _groupRepository.ReadGroupProfileQuestionsOfSetUp(setUpId);

            var groupProfileQuestions = tempList.Where(q => !(q is GroupMCProfileQuestion)).ToList();
            groupProfileQuestions.AddRange(_groupRepository.ReadGroupMcProfileQuestions(setUpId));
            return groupProfileQuestions.ToList();
        }

        public Group AddGroup(int setUpId, string name, Teacher teacher, string groupCode,
            short? maxParticipants)
        {
            var newGroupInfo = new GroupInfo()
            {
                SetUp = _setupRepository.ReadSetupById(setUpId),
                CreationDate = DateTime.Now
            };

            var hasLimit = maxParticipants != 0;

            var newGroup = new Group()
            {
                Name = name,
                HasLimit = hasLimit,
                GroupCode = groupCode,
                Tasks = new List<GroupTask>(),
                Info = newGroupInfo,
                Teacher = teacher,
                MaxParticipants = maxParticipants,
                GroupProfileAnswers = new List<GroupProfileAnswer>()
            };

            teacher.Groups.Add(newGroup);
            _groupRepository.CreateGroup(newGroup);

            return newGroup;
        }
    }
}