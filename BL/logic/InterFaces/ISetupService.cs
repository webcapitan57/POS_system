using System.Collections.Generic;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface ISetupService
    {
        public SetUp ReadSetupById(int id);
        public SetUp ReadSimpleSetupById(int id);
        public SetUp GetSetUp(int id);
        public SetUp GetSetUpDetails(int id);
        public SetUp AddSetUp(SetUp setUp, Admin admin);
        public SetUp UpdateSetUp(SetUp setUp);
        public void RemoveSetUp(int id);
        public void ArchiveSetup(int id);
        public SetUp GetSetUpWithSetTasks(int setupId);
        public bool CheckIfLoginIdentifierIsAlreadyTaken(string identifier);
        public void ChangeLogo(int setUpId, string fileName);
        public void DeleteProfileQuestion(int profileQuestionId);
        public SetUp ReadSetupByCode(string setUpCode);
        public List<StudentProfileQuestion> GetSetUpStudentProfileQuestions(int setUpId);
        public void ClearEmptySetUps();
    }
}