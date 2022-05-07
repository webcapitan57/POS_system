using System.Collections.Generic;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;

namespace POC.DAL.repo.InterFaces
{
    public interface ISetupRepository
    {
        public SetUp ReadSetupById(int id);
        public SetUp ReadSetupWithUsageAndGroupsById(int id);
        public SetUp ReadSetupWithGroupsById(int id);
        public void AddSetup(SetUp setUp);
        public SetUp UpdateSetup(SetUp setUp);
        public void RemoveSetup(SetUp setUp);
        public void AddSetUpAdmin(SetUpAdmin setUpAdmin);
        public SetUp ReadSetUpWithSetTasks(int setupId);
        public string ReadSetupIdentifier(string identifier);
        public SetUp ReadSetupByIdentifier(string identifier);
        public List<StudentProfileQuestion> ReadSetUpStudentProfileQuestions(int setUpId);
        public SetUp ReadSimpleSetupById(int id);
        public void ClearEmptySetUps();
    }
}