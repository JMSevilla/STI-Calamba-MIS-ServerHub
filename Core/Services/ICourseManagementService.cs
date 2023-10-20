using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;

namespace sti_sys_backend.Core.Services;

public interface ICourseManagementService <T> where T : class, ICourseManagement
{
    public Task<dynamic> CreateCategory(GlobalCategories globalCategories);
    public Task<List<GlobalCategories>> ListOfCategories();
    public Task<dynamic> CreateCourse(T entity);
    public Task<List<T>> courseBycateogoryId(Guid id);
    public Task<dynamic> createSubject(SubjectManagement subjectManagement);
    public Task<List<SubjectManagement>> subjectList();
    public Task<List<T>> courseList();
    public Task<dynamic> SelectedSubjectByCourse(SubjectHelper subjectHelper);
    public Task<dynamic> AssignationStudentsToSubjects(SubjectAssignation subjectAssignation);
    public Task<List<SubjectManagement>> AssignedSubjectsByCourse(SubjectHelper subjectHelper);
    public Task<dynamic> DismantlingSubjectsToStudents(Guid id);
    public Task<dynamic> courseManagementList(int courseId);
    public Task<List<SubjectManagement>> subjectsList(int course_id);
}