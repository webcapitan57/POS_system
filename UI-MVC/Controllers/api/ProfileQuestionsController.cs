using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.profile;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/setups/")]
    public class ProfileQuestionsController : ControllerBase
    {
        private readonly ISetupService _setupService;

        public ProfileQuestionsController(ISetupService setupService)
        {
            _setupService = setupService;
        }

        //group
        [HttpGet("{setUpId}/groupprofilequestions/{profileQuestionId}")]
        public IActionResult GetGroupProfileQuestion(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            return Ok(question);
        }
        
        [HttpGet("{setUpId}/groupprofilequestions/{profileQuestionId}/mc")]
        public IActionResult GetGroupMcProfileQuestion(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            return Ok(question);
        }

        [HttpPost("{setUpId}/groupprofilequestions")]
        public IActionResult AddGroupProfileQuestion(int setUpId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = new GroupProfileQuestion()
            {
                Question = "",
                Description = "",
                IsRequired = false,
                SetUp = setUp
            };

            setUp.NeededProfileQuestions.Add(question);
            setUp = _setupService.UpdateSetUp(setUp);
            question = (GroupProfileQuestion)setUp.NeededProfileQuestions.Last();
            return CreatedAtAction("GetGroupProfileQuestion", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId });
        }
        
        [HttpPost("{setUpId}/groupprofilequestions/mc")]
        public IActionResult AddGroupMcProfileQuestion(int setUpId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = new GroupMCProfileQuestion()
            {
                Question = "",
                Description = "",
                IsRequired = false,
                GroupProfileOptions = new List<GroupProfileOption>(),
                SetUp = setUp
            };

            var option1 = new GroupProfileOption()
            {
                Value = "",
                GroupMcProfileQuestion = question
            };
            question.GroupProfileOptions.Add(option1);
            
            var option2 = new GroupProfileOption()
            {
                Value = "",
                GroupMcProfileQuestion = question
            };
            question.GroupProfileOptions.Add(option2);

            setUp.NeededProfileQuestions.Add(question);
            _setupService.UpdateSetUp(setUp);
            return CreatedAtAction("GetGroupMcProfileQuestion", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId });
        }

        [HttpPut("{setUpId}/groupprofilequestions/{profileQuestionId}")]
        public IActionResult UpdateGroupProfileQuestion(int setUpId, int profileQuestionId, [FromBody] ProfileQuestionDTO profileQuestionDto)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            question.Question = profileQuestionDto.Question;
            question.Description = profileQuestionDto.Description;
            question.IsRequired = profileQuestionDto.IsRequired;

            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }

        [HttpDelete("{setUpId}/groupprofilequestions/{profileQuestionId}")]
        public IActionResult DeleteGroupProfileQuestion(int setUpId, int profileQuestionId)
        {
           /* var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.ToList()
                .Find(g => g.ProfileQuestionId == profileQuestionId);
            setUp.NeededProfileQuestions.Remove(question);
            _setupService.UpdateSetUp(setUp);*/

           _setupService.DeleteProfileQuestion(profileQuestionId);
           
            return NoContent();
        }
        
        //student
        [HttpGet("{setUpId}/studentprofilequestions/{profileQuestionId}")]
        public IActionResult GetStudentProfileQuestion(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            return Ok(question);
        }
        
        [HttpGet("{setUpId}/studentprofilequestions/{profileQuestionId}/mc")]
        public IActionResult GetStudentMcProfileQuestion(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            return Ok(question);
        }

        [HttpPost("{setUpId}/studentprofilequestions")]
        public IActionResult AddStudentProfileQuestion(int setUpId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = new StudentProfileQuestion()
            {
                Question = "",
                Description = "",
                IsRequired = false,
                SetUp = setUp
            };

            setUp.NeededProfileQuestions.Add(question);
            setUp = _setupService.UpdateSetUp(setUp);
            question = (StudentProfileQuestion)setUp.NeededProfileQuestions.Last();
            return CreatedAtAction("GetStudentProfileQuestion", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId });
        }
        
        [HttpPost("{setUpId}/studentprofilequestions/mc")]
        public IActionResult AddStudentMcProfileQuestion(int setUpId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = new StudentMCProfileQuestion()
            {
                Question = "",
                Description = "",
                IsRequired = false,
                StudentProfileOptions = new List<StudentProfileOption>(),
                SetUp = setUp
            };

            var option1 = new StudentProfileOption()
            {
                Value = "",
                StudentMcProfileQuestion = question
            };
            question.StudentProfileOptions.Add(option1);
            
            var option2 = new StudentProfileOption()
            {
                Value = "",
                StudentMcProfileQuestion = question
            };
            question.StudentProfileOptions.Add(option2);

            setUp.NeededProfileQuestions.Add(question);
            _setupService.UpdateSetUp(setUp);
            return CreatedAtAction("GetStudentMcProfileQuestion", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId });
        }

        [HttpPut("{setUpId}/studentprofilequestions/{profileQuestionId}")]
        public IActionResult UpdateStudentProfileQuestion(int setUpId, int profileQuestionId, [FromBody] ProfileQuestionDTO profileQuestionDto)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.SingleOrDefault(q=>q.ProfileQuestionId == profileQuestionId);
            question.Question = profileQuestionDto.Question;
            question.Description = profileQuestionDto.Description;
            question.IsRequired = profileQuestionDto.IsRequired;

            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }

        [HttpDelete("{setUpId}/studentprofilequestions/{profileQuestionId}")]
        public IActionResult DeleteStudentProfileQuestion(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = setUp.NeededProfileQuestions.ToList()
                .Find(g => g.ProfileQuestionId == profileQuestionId);
            setUp.NeededProfileQuestions.Remove(question);
            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }
    }
}