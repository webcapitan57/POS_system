using System.Collections.Generic;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class SetupService : ISetupService
    {
        private readonly ISetupRepository _setupRepository;
        private readonly IProfileQuestionRepository _profileQuestionRepository;
        private readonly IPhotoRepository _photoRepository;

        public SetupService(ISetupRepository setupRepository,IProfileQuestionRepository profileQuestionRepository,
            IPhotoRepository photoRepository)
        {
            _setupRepository = setupRepository;
            _profileQuestionRepository = profileQuestionRepository;
            _photoRepository = photoRepository;
        }

        public SetUp GetSetUp(int id)
        {
            var setUp = _setupRepository.ReadSetupById(id);
            return setUp;
        }
        
        public SetUp GetSetUpDetails(int id)
        {
            var setUp = _setupRepository.ReadSetupWithUsageAndGroupsById(id);
            return setUp;
        }

        public SetUp AddSetUp(SetUp setUp, Admin admin)
        {
            _setupRepository.AddSetup(setUp);
            AddSetUpToAdmin(setUp, admin);
            return GetSetUp(setUp.SetUpId);
        }

        private void AddSetUpToAdmin(SetUp setUp, Admin admin)
        {
            var setUpAdmin = new SetUpAdmin()
            {
                SetUp = setUp, Admin = admin
            };

            _setupRepository.AddSetUpAdmin(setUpAdmin);
        }

        public SetUp UpdateSetUp(SetUp setUp)
        {
           return _setupRepository.UpdateSetup(setUp);
            
        }

        public void RemoveSetUp(int id)
        {
            _setupRepository.RemoveSetup(ReadSetupById(id));
        }

        public void ArchiveSetup(int id)
        {
            var setUp = _setupRepository.ReadSetupWithGroupsById(id);
            
            //if archived true -> unarchives -> groups active to true
            //if archived false -> archives -> groups active to false
            if (setUp.Archived)
            {
                setUp.Archived = false;
                
                // Activate groups
                foreach (var teacher in setUp.Teachers)
                {
                    foreach (var group in teacher.Groups)
                    {
                        group.Active = true;
                    }
                }
            }
            else
            {
                setUp.Archived = true;
                
                // Deactivate groups
                foreach (var teacher in setUp.Teachers)
                {
                    foreach (var group in teacher.Groups)
                    {
                        group.Active = false;
                    }
                }
            }

            _setupRepository.UpdateSetup(setUp);
        }

        public SetUp GetSetUpWithSetTasks(int setupId)
        {
            return _setupRepository.ReadSetUpWithSetTasks(setupId);
        }

        public bool CheckIfLoginIdentifierIsAlreadyTaken(string identifier)
        {
            return _setupRepository.ReadSetupIdentifier(identifier)==null ;
        }

        public void ChangeLogo(int setUpId,string fileName)
        {
            var logoToChange = _photoRepository.ReadSetupLogo(setUpId);
            logoToChange.Picture = fileName;
            _photoRepository.UpdatePhoto(logoToChange);

        }

        public void DeleteProfileQuestion(int profileQuestionId)
        {
            var profileQuestion = _profileQuestionRepository.ReadProfileQuestionById(profileQuestionId);
            _profileQuestionRepository.RemoveProfileQuestion(profileQuestion);
            
        }

        public SetUp ReadSetupByCode(string identifier)
        {
           return _setupRepository.ReadSetupByIdentifier(identifier);
        }

        public List<StudentProfileQuestion> GetSetUpStudentProfileQuestions(int setUpId)
        {
            return _setupRepository.ReadSetUpStudentProfileQuestions(setUpId);
        }

        public void ClearEmptySetUps()
        {
            _setupRepository.ClearEmptySetUps();
        }

        public SetUp ReadSetupById(int id)
        {
            return _setupRepository.ReadSetupById(id);
        }
        
        public SetUp ReadSimpleSetupById(int id)
        {
            return _setupRepository.ReadSimpleSetupById(id);
        }
    }
}