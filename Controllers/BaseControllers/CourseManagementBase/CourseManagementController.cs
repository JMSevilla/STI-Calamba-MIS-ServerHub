using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;

namespace sti_sys_backend.Controllers.BaseControllers.CourseManagementBase;
[ApiController]
[ServiceFilter(typeof(KeyAuthFilter))]
[Route("/api/v1/[controller]")]
public abstract class CourseManagementController<TEntity, TRepository> : ControllerBase
    where TEntity : class, ICourseManagement
    where TRepository : ICourseManagementService<TEntity>
{
    private readonly TRepository _repository;

    public CourseManagementController(TRepository repository)
    {
        _repository = repository;
    }

    [Route("create-category"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCategory([FromBody] GlobalCategories globalCategories)
    {
        var result = await _repository.CreateCategory(globalCategories);
        return Ok(result);
    }

    [Route("category-list"), HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(List<GlobalCategories>)))]
    public async Task<IActionResult> ListOfCategories()
    {
        List<GlobalCategories> list = await _repository.ListOfCategories();
        return Ok(list);
    }

    [Route("create-course"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCourse([FromBody] TEntity entity)
    {
        var result = await _repository.CreateCourse(entity);
        return Ok(result);
    }

    [Route("course-by-category/{id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CourseByCategory([FromRoute] Guid id)
    {
        var result = await _repository.courseBycateogoryId(id);
        return Ok(result);
    }

    [Route("subject-list"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SubjectList()
    {
        List<SubjectManagement> list = await _repository.subjectList();
        return Ok(list);
    }

    [Route("create-subject"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SubjectCreation([FromBody] SubjectManagement subjectManagement)
    {
        var result = await _repository.createSubject(subjectManagement);
        return Ok(result);
    }

    [Route("course-list"), HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CourseManagement>))]
    public async Task<IActionResult> CourseList()
    {
        List<TEntity> list = await _repository.courseList();
        return Ok(list);
    }

    [Route("selected-subject-on-course"), HttpGet, HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SelectedSubjectOnCourse([FromBody] SubjectHelper subjectHelper)
    {
        var result = await _repository.SelectedSubjectByCourse(subjectHelper);
        return Ok(result);
    }

    [Route("assigned-subject-on-course"), HttpGet, HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AssignedSubjectOnCourse([FromBody] SubjectHelper subjectHelper)
    {
        var result = await _repository.AssignedSubjectsByCourse(subjectHelper);
        return Ok(result);
    }
    [Route("subject-start-assignation"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SubjectStartAssignation([FromBody] SubjectAssignation subjectAssignation)
    {
        var result = await _repository.AssignationStudentsToSubjects(subjectAssignation);
        return Ok(result);
    }

    [Route("dismantle-subjects-to-students/{subjectId}"), HttpDelete]
    [AllowAnonymous]
    public async Task<IActionResult> DismantleSubjectsToStudents([FromRoute] Guid subjectId)
    {
        var result = await _repository.DismantlingSubjectsToStudents(subjectId);
        return Ok(result);
    }

    [Route("course-list-viewing/{course_id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CourseViewingList([FromRoute] int course_id)
    {
        var result = await _repository.courseManagementList(course_id);
        return Ok(result);
    }

    [Route("subjects-list-by-course/{course_id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> SubjectsListByCourse([FromRoute] int course_id)
    {
        var result = await _repository.subjectsList(course_id);
        return Ok(result);
    }
}