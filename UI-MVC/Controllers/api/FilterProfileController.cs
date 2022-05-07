using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Models.ModerateDTO;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/filterprofiles")]
    public class FilterProfileController : ControllerBase
    {
        private IUserService _userService;

        public FilterProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{filterProfileId:int}/filters")]
        public IActionResult GetFilters(int filterProfileId)
        {
            var filters = _userService.GetFilterProfile(filterProfileId).Filters;

            return Ok(filters);
        }

        [HttpPut("")]
        public IActionResult AddFilterProfile([FromBody] FilterProfileDTO filterProfileDto)
        {
            var filterProfile = new FilterProfile()
            {
                ProfileName = filterProfileDto.Name,
                User = _userService.GetUser(filterProfileDto.UserId),
                Filters = new List<Filter>()
            };

            foreach (var newFilter in filterProfileDto.Filters.Select(filter => new Filter()
            {
                Description = filter.Key,
                Value = filter.Value
            }))
            {
                filterProfile.Filters.Add(newFilter);
            }
            
            _userService.AddFilterProfile(filterProfile);

            return Ok(filterProfile);
        }
    }
}