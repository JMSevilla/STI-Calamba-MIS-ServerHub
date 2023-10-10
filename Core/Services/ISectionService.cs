using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;

namespace sti_sys_backend.Core.Services
{
    public interface ISectionService<T> where T : class, ISections
    {
        public Task<dynamic> createSection(T section);
        public Task<dynamic> findAllSections();

        public Task<int> findBigId();
        public Task<dynamic> createCourses(Courses courses);
        public Task<dynamic> findAllCourses();
        public Task<List<Courses>> findAllCoursesByAcronyms(string acronyms);
        public Task<dynamic> RemoveCourse(int id);
        public Task<List<Courses>> getAllCoursesNonJoined();
        public Task<List<T>> getAllSectionsNonJoined();
    }
}
