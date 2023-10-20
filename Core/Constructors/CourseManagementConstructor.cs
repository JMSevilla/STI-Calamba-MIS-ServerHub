using sti_sys_backend.Core.ServiceImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;

namespace sti_sys_backend.Core.Constructors;

public class CourseManagementConstructor : CourseManagementServiceImpl<CourseManagement, DatabaseQueryable>
{
    public CourseManagementConstructor(DatabaseQueryable context) : base(context)
    {
    }
}