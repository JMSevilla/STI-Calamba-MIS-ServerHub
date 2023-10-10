using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.BaseControllers.TicketBase
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(KeyAuthFilter))]
    public abstract class TicketingBaseController<TEntity, TRepository> : ControllerBase
        where TEntity : class, ITicketIssues
        where TRepository : ITicketingService<TEntity>
    {
        private readonly TRepository _repository;
        protected TicketingBaseController(TRepository repository)
        {
            _repository = repository;
        }

        [Route("add-ticket-issue"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(int), 200)]
        public virtual async Task<IActionResult> AddNewTicketIssue(TEntity ticket)
        {
            var result = (await _repository.addTicketIssues(ticket));
            return Ok(result);
        }

        [Route("list-issues"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketIssues>))]
        public async Task<IActionResult> ListOfIssues()
        {
            List<TEntity> result = await _repository.listIssues();
            return Ok(result);
        }

        [Route("add-com-labs"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddComLabs([FromBody] ComLaboratory comLaboratory)
        {
            var result = await _repository.addComLabs(comLaboratory);
            return Ok(result);
        }
        
        [Route("add-pcs"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> AddPcs([FromBody] PC pc)
        {
            var result = await _repository.addPcs(pc);
            return Ok(result);
        }
        [Route("list-pcs"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PC>))]
        public async Task<IActionResult> ListOfPcs()
        {
            List<PC> result = await _repository.ListOfPcs();
            return Ok(result);
        }
        [Route("list-com-labs"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComLaboratory>))]
        public async Task<IActionResult> ListOfComLabs()
        {
            List<ComLaboratory> result = await _repository.listOfComLabs();
            return Ok(result);
        }

        [Route("filtered-list-of-pcs/{comlabId}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PC>))]
        public async Task<IActionResult> FilteredPcs([FromRoute] Guid comlabId)
        {
            List<PC> list = (await _repository.FilteredListOfPcs(comlabId));
            return Ok(list);
        }

        [Route("create-ticket"), HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateTicket([FromBody] Ticketing ticketing)
        {
            var result = (await _repository.CreateTicket(ticketing));
            return Ok(result);
        }

        [Route("all-ticket-list"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ticketing>))]
        public async Task<IActionResult> AllTickets()
        {
            List<Ticketing> list = (await _repository.AllTicketsList());
            return Ok(list);
        }

        [Route("current-user-tickets/{id}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ticketing>))]
        public async Task<IActionResult> CurrentAccountTickets([FromRoute] int id)
        {
            List<Ticketing> currList = (await _repository.CurrentUserTickets(id));
            return Ok(currList);
        }

        [Route("current-user-admin-tickets/{id}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ticketing>))]
        public async Task<IActionResult> CurrentAdminAccountTickets([FromRoute] int id)
        {
            List<Ticketing> currList = (await _repository.CurrentUserAdminTickets(id));
            return Ok(currList);
        }

        [Route("all-tickets-by-status/{issueStatuses}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ticketing>))]
        public async Task<IActionResult> AllTicketsByStatus([FromRoute] IssueStatuses issueStatuses)
        {
            List<Ticketing> list = (await _repository.AllTicketsByStatuses(issueStatuses));
            return Ok(list);
        }

        [Route("current-ticket-status-search-engine/{id}/{issueStatuses}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Ticketing>))]
        public async Task<IActionResult> CurrentTicketByStatus([FromRoute] int id,
            [FromRoute] IssueStatuses issueStatuses)
        {
            List<Ticketing> list = (await _repository.CurrentTicketStatusSearchEngine(id, issueStatuses));
            return Ok(list);
        }

        [Route("find-progress-by-status/{id}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IssueStatuses))]
        public async Task<IActionResult> FindProgressByStatus([FromRoute] Guid id)
        {
            IssueStatuses issueStatuses = (await _repository.FindCurrentStatusById(id));
            return Ok(issueStatuses);
        }

        [Route("remove-ticket/{id}"), HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteTicketById([FromRoute] Guid id)
        {
            bool deletedEntity = (await _repository.DeleteTicketById(id));
            return Ok(deletedEntity);
        }

        [Route("fetch-tickets-to-notification"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FetchedTicketsToNotification()
        {
            var notifs = (await _repository.FetchTicketsByPushNotifs());
            return Ok(notifs);
        }

        [Route("fetch-tickets-to-notification-in-progress"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FetchTicketsInprogress()
        {
            var inprogressNotifs = (await _repository.FetchTicketsByPushNotifsInProgress());
            return Ok(inprogressNotifs);
        }


        [Route("filtered-tickets-from-push-notif/{id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FilteredTicketsFromPushNotif([FromRoute] Guid id)
        {
            var list = (await _repository.FilteredTicketsFromPushNotification(id));
            return Ok(list);
        }

        [Route("find-comlab-by-guid/{id}"), HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ComLaboratory>))]
        public async Task<IActionResult> FindComlabByGuid([FromRoute] Guid id)
        {
            List<ComLaboratory> list = (await _repository.listOfComLabsByGuid(id));
            return Ok(list);
        }

        [Route("assign-to-current-user/{ticketId}/uid/{id}"), HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> AssignToCurrent([FromRoute] Guid ticketId, [FromRoute] int id)
        {
            var result = (await _repository.AssignToCurrentUser(ticketId, id));
            return Ok(result);
        }

        [Route("change-status-from-ticket/{ticketId}/{status}"), HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> ChangeStatusFromTicket([FromRoute] Guid ticketId, [FromRoute] IssueStatuses status)
        {
            var result = (await _repository.ChangeStatusFromTicketDetails(ticketId, status));
            return Ok(result);
        }

        [Route("remove-pc/{id}"), HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemovePC([FromRoute] Guid id)
        {
            var res = await _repository.RemovePC(id);
            return Ok(res);
        }
        
        [Route("remove-comlab/{id}"), HttpDelete]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> RemoveComlab([FromRoute] Guid id)
        {
            var res = await _repository.RemoveComLab(id);
            return Ok(res);
        }
        /* Report Tickets */
        [Route("total-open-tickets-report/{type}/{section}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TotalCounts([FromRoute] string type, [FromRoute] int? section)
        {
            var result = await _repository.TotalReports(type, section);
            return Ok(result);
        }

        [Route("ticket-report-chart"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TicketReportChart()
        {
            var result = await _repository.AdminReportChartTickets();
            return Ok(result);
        }

        [Route("fetch-pc-by-comlab-id/{id}"), HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPCByComlabId([FromRoute] Guid id)
        {
            var result = await _repository.FetchPCsUnderComlabId(id);
            return Ok(result);
        }
    }
}
