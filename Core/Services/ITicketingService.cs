using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;

namespace sti_sys_backend.Core.Services
{
    public interface ITicketingService<T> where T : class, ITicketIssues
    {
        public Task<dynamic> addTicketIssues(T entity);
        public Task<List<T>> listIssues();
        public Task<dynamic> addComLabs(ComLaboratory comLaboratory);
        public Task<List<ComLaboratory>> listOfComLabs();
        public Task<List<ComLaboratory>> listOfComLabsByGuid(Guid id);
        public Task<dynamic> addPcs(PC pc);
        public Task<List<PC>> ListOfPcs();
        public Task<List<PC>> FilteredListOfPcs(Guid comlabId);
        public Task<dynamic> CreateTicket(Ticketing ticketing);
        public Task<List<Ticketing>> AllTicketsList();
        public Task<List<Ticketing>> CurrentUserTickets(int id);
        public Task<List<Ticketing>> CurrentUserAdminTickets(int id);
        public Task<List<Ticketing>> AllTicketsByStatuses(IssueStatuses statuses);
        public Task<List<Ticketing>> CurrentTicketStatusSearchEngine(int id, IssueStatuses statuses);
        public Task<IssueStatuses> FindCurrentStatusById(Guid id);
        public Task<bool> DeleteTicketById(Guid id);
        public Task<dynamic> FetchTicketsByPushNotifs();
        public Task<dynamic> FetchTicketsByPushNotifsInProgress();
        public Task<dynamic> FilteredTicketsFromPushNotification(Guid id);

        public Task<dynamic> AssignToCurrentUser(Guid ticketId, int id);
        public Task<dynamic> ChangeStatusFromTicketDetails(Guid ticketId, IssueStatuses status);
        public Task<dynamic> RemovePC(Guid id);
        public Task<dynamic> RemoveComLab(Guid id);
        public Task<dynamic> TotalReports(TicketReportsHelper ticketReportsHelper);

        public Task<dynamic> AdminReportChartTickets(); //test-env
        public Task<dynamic> FetchPCsUnderComlabId(Guid comlabId);
        
    }
}
