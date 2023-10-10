using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.BaseControllers.SectionBase
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(KeyAuthFilter))]
    public abstract class SectionBaseController<TEntity, TRepository> : ControllerBase
        where TEntity : class, ISections
        where TRepository : ISectionService<TEntity>
    {
        private readonly TRepository _repository;
        protected SectionBaseController(TRepository repository)
        {
            _repository = repository;
        }

        [Route("create-section"), HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> createNewSection(TEntity entity)
        {
            var result = (await _repository.createSection(entity));
            return Ok(result);
        }

        [Route("get-all-sections"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Sections>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllSections()
        {
            var sections = await _repository.findAllSections();
            return Ok(sections);
        }

        [Route("find-big-id"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindBigId()
        {
            var bigId = await _repository.findBigId();
            if(bigId == 0)
            {
                return Ok(0);
            }
            return Ok(bigId);
        }

        [Route("create-course"), HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Courses>))]
        public async Task<IActionResult> CreateNewCourses([FromBody] Courses courses)
        {
            var result = (await _repository.createCourses(courses));
            return Ok(result);
        }

        [Route("get-list-courses"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Courses>))]
        public async Task<IActionResult> GetCoursesList()
        {
            var listOfCourses = (await _repository.findAllCourses());
            return Ok(listOfCourses);
        }

        [Route("find-courses-by-acronyms/{acronyms}"), HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Courses>))]
        public async Task<IActionResult> FindCoursesByAcronyms([FromRoute] string acronyms)
        {
            List<Courses> courses = (await _repository.findAllCoursesByAcronyms(acronyms));
            return Ok(courses);
        }

        [Route("remove-course/{id}"), HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveCourse([FromRoute] int id)
        {
            var result = await _repository.RemoveCourse(id);
            return Ok(result);
        }

        [Route("get-all-courses"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCourses()
        {
            var result = await _repository.getAllCoursesNonJoined();
            return Ok(result);
        }

        [Route("get-all-sections-non-joined"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSectionsNonJoined()
        {
            var result = await _repository.getAllSectionsNonJoined();
            return Ok(result);
        }
    }
}
