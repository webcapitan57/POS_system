using Microsoft.AspNetCore.Mvc;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [Route("/api/students")]   
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;

        public StudentsController(IStudentService studentService, IGroupService groupService)
        {
            _studentService = studentService;
            _groupService = groupService;
        }

        [HttpPost("{groupCode}")]
        public IActionResult AddStudent(string groupCode)
        {
            var group = _groupService.GetGroup(groupCode);
            var student = _studentService.AddStudent(group);
            return Ok(student.StudentId);
        }
    }
}