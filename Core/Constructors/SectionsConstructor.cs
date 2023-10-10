using sti_sys_backend.Core.ServiceImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;

namespace sti_sys_backend.Core.Constructors
{
    public class SectionsConstructor: SectionImpl<Sections, DatabaseQueryable>
    {
        public SectionsConstructor(DatabaseQueryable context) : base(context) { }
    }
}
