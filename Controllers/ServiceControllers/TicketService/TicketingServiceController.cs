using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers.TicketBase;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.TicketService
{

    public class TicketingServiceController : TicketingBaseController<TicketIssues, TicketingConstructor>
    {
        public TicketingServiceController(TicketingConstructor repository) : base(repository)
        {
        }
    }
}
