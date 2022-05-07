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
    public class ProfileOptionsController : ControllerBase
    {
        private readonly ISetupService _setupService;

        public ProfileOptionsController(ISetupService setupService)
        {
            _setupService = setupService;
        }
        
        //group
        [HttpGet("{setUpId}/groupprofilequestions/{profileQuestionId}/groupoptions/{profileOptionId}")]
        public IActionResult GetGroupOption(int setUpId, int profileQuestionId, int profileOptionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = question.GroupProfileOptions.SingleOrDefault(o=>o.ProfileOptionId == profileOptionId);
            return Ok(option);
        }

        [HttpGet("{setUpId}/groupprofilequestions/{profileQuestionId}/groupoptions")]
        public IActionResult GetGroupOptions(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var options = question.GroupProfileOptions.AsEnumerable();
            return Ok(options);
        }

        [HttpPost("{setUpId}/groupprofilequestions/{profileQuestionId}/groupoptions")]
        public IActionResult AddGroupOption(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = new GroupProfileOption()
            {
                Value = "",
                GroupMcProfileQuestion = question
            };
            
            question.GroupProfileOptions.Add(option);

            setUp = _setupService.UpdateSetUp(setUp);
            question = (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                q.ProfileQuestionId == profileQuestionId);
            option = question.GroupProfileOptions.Last();
            return CreatedAtAction("GetGroupOption", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId,  profileOptionId = option.ProfileOptionId});
        }

        [HttpPut("{setUpId}/groupprofilequestions/{profileQuestionId}/groupoptions/{profileOptionId}")]
        public IActionResult UpdateGroupOption(int setUpId, int profileQuestionId, int profileOptionId, [FromBody] ProfileOptionDTO profileOptionDto)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (GroupMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = question.GroupProfileOptions.SingleOrDefault(o => o.ProfileOptionId == profileOptionId);
            option.Value = profileOptionDto.Value;
            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }

        [HttpDelete("{setUpId}/groupprofilequestions/{profileQuestionId}/groupoptions/{profileOptionId}")]
        public IActionResult DeleteGroupProfileOption(int setUpId, int profileQuestionId, int profileOptionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = (GroupMCProfileQuestion) setUp.NeededProfileQuestions.ToList()
                .FirstOrDefault(g => g.ProfileQuestionId == profileQuestionId);

            var option = question.GroupProfileOptions.ToList().Find(o => o.ProfileOptionId == profileOptionId);

            setUp.NeededProfileQuestions.Remove(question);
            question.GroupProfileOptions.Remove(option);

            setUp.NeededProfileQuestions.Add(question);
            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }
        
        //student
        [HttpGet("{setUpId}/studentprofilequestions/{profileQuestionId}/studentoptions/{profileOptionId}")]
        public IActionResult GetStudentOption(int setUpId, int profileQuestionId, int profileOptionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = question.StudentProfileOptions.SingleOrDefault(o=>o.ProfileOptionId == profileOptionId);
            return Ok(option);
        }

        [HttpGet("{setUpId}/studentprofilequestions/{profileQuestionId}/studentoptions")]
        public IActionResult GetStudentOptions(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var options = question.StudentProfileOptions.AsEnumerable();
            return Ok(options);
        }

        [HttpPost("{setUpId}/studentprofilequestions/{profileQuestionId}/studentoptions")]
        public IActionResult AddStudentOption(int setUpId, int profileQuestionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = new StudentProfileOption()
            {
                Value = "",
                StudentMcProfileQuestion = question
            };
            
            question.StudentProfileOptions.Add(option);

            setUp = _setupService.UpdateSetUp(setUp);
            question = (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                q.ProfileQuestionId == profileQuestionId);
            option = question.StudentProfileOptions.Last();
            return CreatedAtAction("GetStudentOption", new{setUpId = setUp.SetUpId, profileQuestionId = question.ProfileQuestionId,  profileOptionId = option.ProfileOptionId});
        }

        [HttpPut("{setUpId}/studentprofilequestions/{profileQuestionId}/studentoptions/{profileOptionId}")]
        public IActionResult UpdateStudentOption(int setUpId, int profileQuestionId, int profileOptionId, [FromBody] ProfileOptionDTO profileOptionDto)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question =
                (StudentMCProfileQuestion) setUp.NeededProfileQuestions.SingleOrDefault(q =>
                    q.ProfileQuestionId == profileQuestionId);
            var option = question.StudentProfileOptions.SingleOrDefault(o => o.ProfileOptionId == profileOptionId);
            option.Value = profileOptionDto.Value;
            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }

        [HttpDelete("{setUpId}/studentprofilequestions/{profileQuestionId}/studentoptions/{profileOptionId}")]
        public IActionResult DeleteStudentProfileOption(int setUpId, int profileQuestionId, int profileOptionId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            var question = (StudentMCProfileQuestion) setUp.NeededProfileQuestions.ToList()
                .FirstOrDefault(g => g.ProfileQuestionId == profileQuestionId);

            var option = question.StudentProfileOptions.ToList().Find(o => o.ProfileOptionId == profileOptionId);

            setUp.NeededProfileQuestions.Remove(question);
            question.StudentProfileOptions.Remove(option);

            setUp.NeededProfileQuestions.Add(question);
            _setupService.UpdateSetUp(setUp);
            return NoContent();
        }
    }
}