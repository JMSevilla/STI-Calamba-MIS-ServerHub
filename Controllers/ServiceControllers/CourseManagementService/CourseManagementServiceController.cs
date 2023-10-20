using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers.CourseManagementBase;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.CourseManagementService;

public class CourseManagementServiceController : CourseManagementController<CourseManagement, CourseManagementConstructor>
{
    public CourseManagementServiceController(CourseManagementConstructor repository) : base(repository)
    {
    }
}