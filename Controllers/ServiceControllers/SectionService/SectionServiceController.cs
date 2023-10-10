using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers.SectionBase;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.SectionService
{
    public class SectionServiceController : SectionBaseController<Sections, SectionsConstructor>
    {
        public SectionServiceController(SectionsConstructor repository) : base(repository)
        {
        }
    }
}
