using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;

namespace sti_sys_backend.Core.ServiceImplementations;

public abstract class CourseManagementServiceImpl<TEntity, TContext> : ICourseManagementService<TEntity>
    where TEntity : class, ICourseManagement
    where TContext : DatabaseQueryable
{
    private readonly TContext _context;
    public CourseManagementServiceImpl(TContext context)
    {
        _context = context;
    }

    public async Task<dynamic> CreateCategory(GlobalCategories globalCategories)
    {
        bool checkCategories = await _context.Set<GlobalCategories>()
            .AnyAsync(x => x.categoryName == globalCategories.categoryName);
        if (checkCategories)
        {
            return 400;
        }

        WorldTimeAPI worldTimeApi = new WorldTimeAPI();
        DateTime currentDate = await worldTimeApi.ConfigureDateTime();
        globalCategories._categoryStatus = CategoryStatus.ACTIVE;
        globalCategories.categoryPath = "none";
        globalCategories.created_at = currentDate;
        globalCategories.updated_at = currentDate;
        await _context.Set<GlobalCategories>().AddAsync(globalCategories);
        await _context.SaveChangesAsync();
        return 200;
    }

    public async Task<List<GlobalCategories>> ListOfCategories()
    {
        List<GlobalCategories> list = await _context.GlobalCategoriesEnumerable
            .Where(x => x._categoryStatus == CategoryStatus.ACTIVE)
            .ToListAsync();
        return list;
    }

    public async Task<dynamic> CreateCourse(TEntity entity)
    {
        bool courseExists = await _context.Set<TEntity>()
            .AnyAsync(x => x.courseName == entity.courseName && x.courseAcronym == entity.courseAcronym);
        if (courseExists)
        {
            return 400;
        }
        else
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            entity._courseStatus = CourseStatus.ACTIVE;
            entity._courseSlotStatus = CourseSlotStatus.HAS_AVAILABLE_SLOT;
            entity.created_at = currentDate;
            entity.updated_at = currentDate;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return 200;
        }
    }

    public async Task<List<TEntity>> courseBycateogoryId(Guid id)
    {
        List<TEntity> list = await _context.Set<TEntity>()
            .Where(x => x.categoryId == id).ToListAsync();
        return list;
    }

    public async Task<dynamic> createSubject(SubjectManagement subjectManagement)
    {
        WorldTimeAPI worldTimeApi = new WorldTimeAPI();
        bool checkSubject = await _context
            .SubjectManagements.AnyAsync(x => x.subjectName
                                              == subjectManagement.subjectName);
        if (checkSubject)
        {
            return 400;
        }
        DateTime currentDate = await worldTimeApi.ConfigureDateTime();
        subjectManagement.created_at = currentDate;
        subjectManagement.updated_at = currentDate;
        await _context.SubjectManagements.AddAsync(subjectManagement);
        await _context.SaveChangesAsync();
        return 200;
    }

    public async Task<List<SubjectManagement>> subjectList()
    {
        List<SubjectManagement> list = await _context
            .Set<SubjectManagement>()
            .ToListAsync();
        return list;
    }

    public async Task<List<TEntity>> courseList()
    {
        List<TEntity> list = await _context.Set<TEntity>()
            .Where(x => x._courseStatus == CourseStatus.ACTIVE
                        && x._courseSlotStatus == CourseSlotStatus.HAS_AVAILABLE_SLOT)
            .ToListAsync();
        return list;
    }

    public async Task<dynamic> SelectedSubjectByCourse(SubjectHelper subjectHelper)
    {
        // validate this implementation, prevent showing the assigned subjects inside the subjects list
        var assignedSubjectIds = _context.SubjectAssignations
            .Where(sa => subjectHelper.accountId.Contains(sa.accountId) && sa.courseId == subjectHelper.courseId)
            .Select(sa => sa.subjectId)
            .ToList();

        var list = _context.SubjectManagements
            .Where(sm => sm.courseId == subjectHelper.courseId)
            .Where(sm => !assignedSubjectIds.Contains(sm.id))
            .ToList();

        return list;
    }

    public async Task<dynamic> AssignationStudentsToSubjects(SubjectAssignation subjectAssignation)
    {
        
        bool _checkAssignations = await _context.SubjectAssignations.AnyAsync(x =>
            x.subjectId == subjectAssignation.subjectId
            && x.accountId == subjectAssignation.accountId);
        if (_checkAssignations)
        {
            return 400;
        }
        WorldTimeAPI worldTimeApi = new WorldTimeAPI();
        DateTime currentDate = await worldTimeApi.ConfigureDateTime();
        subjectAssignation.created_at = currentDate;
        subjectAssignation.updated_at = currentDate;
        await _context.SubjectAssignations.AddAsync(subjectAssignation);
        await _context.SaveChangesAsync();
        return 200;
    }

    public async Task<List<SubjectManagement>> AssignedSubjectsByCourse(SubjectHelper subjectHelper)
    {
        var assignedSubjectIds = _context.SubjectAssignations
            .Where(sa => subjectHelper.accountId.Contains(sa.accountId) && sa.courseId == subjectHelper.courseId)
            .Select(sa => sa.subjectId)
            .ToList();

        var list = _context.SubjectManagements
            .Where(sm => sm.courseId == subjectHelper.courseId)
            .Where(sm => assignedSubjectIds.Contains(sm.id))
            .ToList();

        return list;
    }

    public async Task<dynamic> DismantlingSubjectsToStudents(Guid id)
    {
        var _lookInSubjectAssignations = await _context.SubjectAssignations
            .Where(x => x.subjectId == id)
            .FirstOrDefaultAsync();
        if (_lookInSubjectAssignations != null)
        {
            _context.SubjectAssignations.Remove(_lookInSubjectAssignations);
            await _context.SaveChangesAsync();
            return 200;
        }
        return 400;
    }

    public async Task<dynamic> courseManagementList(int courseId)
    {
        var lookForStudentsUnderCourse = await _context.AccountsEnumerable
            .Where(x => x.course_id == courseId && x.access_level == 3).CountAsync();
        
        var list = await _context.Set<TEntity>()
            .Where(x => x.id == courseId)
            .ToListAsync();
        
        return new
        {
            Course = list,
            Nums = lookForStudentsUnderCourse
        };
    }

    public async Task<List<SubjectManagement>> subjectsList(int course_id)
    {
        List<SubjectManagement> list = await _context.SubjectManagements
            .Where(x => x.courseId == course_id)
            .ToListAsync();
        return list;
    }
}