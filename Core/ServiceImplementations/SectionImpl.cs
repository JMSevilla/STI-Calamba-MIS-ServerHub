using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;

namespace sti_sys_backend.Core.ServiceImplementations
{
    public abstract class SectionImpl<TEntity, TContext> : ISectionService<TEntity>
        where TEntity : class, ISections
        where TContext : DatabaseQueryable
    {
        private readonly TContext _context;
        protected SectionImpl(TContext context)
        {
            _context = context;
        }

        public async Task<dynamic> createCourses(Courses courses)
        {
            bool checkCourses = await _context.Set<Courses>()
                .AnyAsync(x => x.course == courses.course || x.courseAcronym == courses.courseAcronym);
            var findCoursesExists = await _context.Set<Courses>()
                .Where(x => x.course == courses.course || x.courseAcronym == courses.courseAcronym)
                .ToListAsync();
            if(!checkCourses)
            {
                courses.createdAt = DateTime.Now;
                courses.updatedAt = DateTime.Now;
                await _context.Set<Courses>().AddAsync(courses);
                await _context.SaveChangesAsync();
                return 200;
            }
            return findCoursesExists;
        }

        public async Task<dynamic> createSection(TEntity section)
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            bool checkSectionExists = await _context.Set<TEntity>().AnyAsync(
                x => x.section_id == section.section_id || x.sectionName == section.sectionName);
            if (checkSectionExists)
            {
                return 403;
            }
            section.created_at = currentDate;
            section.updated_at = currentDate;
            await _context.Set<TEntity>().AddAsync(section);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<dynamic> findAllCourses()
        {
            var courses = await _context.CoursesEnumerable
                .Select(p => new
                {
                    Course = p,
                    Account = _context.AccountsEnumerable
                        .Where(x => x.course_id == p.id)
                        .ToList()
                }).ToListAsync();
            return courses;
        }

        public async Task<List<Courses>> findAllCoursesByAcronyms(string acronyms)
        {
            List<Courses> coursesByAcronyms = await _context.Set<Courses>()
                .Where(x => x.courseAcronym.Contains(acronyms))
                .ToListAsync() .ConfigureAwait(false);
            return coursesByAcronyms;
        }

        public async Task<dynamic> RemoveCourse(int id)
        {
            var courseFound = await _context.Set<Courses>()
                .Where(x => x.id == id)
                .FirstOrDefaultAsync();
            if (courseFound != null)
            {
                _context.Set<Courses>().Remove(courseFound);
                await _context.SaveChangesAsync();
                return 200;
            }

            return 400;
        }

        public async Task<List<Courses>> getAllCoursesNonJoined()
        {
            List<Courses> allCourses = await _context.Set<Courses>()
                .ToListAsync();
            return allCourses;
        }

        public async Task<List<TEntity>> getAllSectionsNonJoined()
        {
            List<TEntity> allSections = await _context.Set<TEntity>()
                .ToListAsync();
            return allSections;
        }

        public async Task<dynamic> findAllSections()
        {
            var sectionsList = await _context.Set<TEntity>()
                .Where(x => x.status == 1)
                .Select(entities => new
                {
                    Section = entities,
                    Accounts = _context.AccountsEnumerable.Where(p => p.section == entities.id).ToList()
                })
                .OrderByDescending(item => item.Section.created_at).ToListAsync();
            return sectionsList;
        }

        public async Task<int> findBigId()
        {
           var entityWithHighestId = await _context.Set<TEntity>()
                .OrderByDescending(x => x.id)
                .FirstOrDefaultAsync();
            return entityWithHighestId == null ? 0 : entityWithHighestId.id;
        }
    }
}
