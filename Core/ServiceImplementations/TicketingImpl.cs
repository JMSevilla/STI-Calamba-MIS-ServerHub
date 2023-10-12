using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using Accounts = sti_sys_backend.Models.Accounts;

namespace sti_sys_backend.Core.ServiceImplementations
{
    public abstract class TicketingImpl<TEntity, TContext> : ITicketingService<TEntity>
        where TEntity : class, ITicketIssues
        where TContext : DatabaseQueryable
    {
        private readonly TContext _context;
        public TicketingImpl(TContext context) { 
            _context = context;
        }

        public async Task<dynamic> addTicketIssues(TEntity entity)
        {
           bool checkTicketIssues = await _context.Set<TEntity>().AnyAsync(x => x.issue == entity.issue);
            if (checkTicketIssues)
            {
                return 403;
            }
            entity.created_at = DateTime.Now;
            entity.updated_at = DateTime.Now;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<List<TEntity>> listIssues()
        {
           List<TEntity> list = await _context.Set<TEntity>()
                .Where(x => x.status == 1).ToListAsync();
            return list;
        }

        public async Task<dynamic> addComLabs(ComLaboratory comLaboratory)
        {
            bool checkExistingLabs = await _context.Set<ComLaboratory>()
                .AnyAsync(x => x.comlabName == comLaboratory.comlabName);
            if (checkExistingLabs)
            {
                return 403;
            }

            comLaboratory.totalComputers = 0;
            comLaboratory.totalNotWorkingComputers = 0;
            comLaboratory.totalNoNetworkComputers = 0;
            await _context.Set<ComLaboratory>().AddAsync(comLaboratory);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<List<ComLaboratory>> listOfComLabs()
        {
            List<ComLaboratory> list = await _context
                .Set<ComLaboratory>()
                .ToListAsync();
            return list;
        }

        public async Task<dynamic> addPcs(PC pc)
        {
            bool checkExistingLabs = await _context.Set<PC>()
                .AnyAsync(x => x.computerName == pc.computerName);
            if (checkExistingLabs)
            {
                return 403;
            }

            pc.computerStatus = 0;
            await _context.Set<PC>().AddAsync(pc);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<List<PC>> ListOfPcs()
        {
            List<PC> list = await _context
                .Set<PC>()
                .ToListAsync();
            return list;
        }

        public async Task<List<PC>> FilteredListOfPcs(Guid comlabId)
        {
            List<PC> pcs = await _context
                .Set<PC>()
                .Where(x => x.comlabId == comlabId)
                .ToListAsync();
            return pcs;
        }

        public async Task<dynamic> CreateTicket(Ticketing ticketing)
        {
            WorldTimeAPI worldTimeApi = new WorldTimeAPI();
            DateTime currentDate = await worldTimeApi.ConfigureDateTime();
            ticketing.pushNotif = 1;
            ticketing.created_at = currentDate;
            ticketing.updated_at = currentDate;
            await _context.Set<Ticketing>().AddAsync(ticketing);
            await _context.SaveChangesAsync();
            return 200;
        }

        public async Task<List<Ticketing>> AllTicketsList()
        {
            List<Ticketing> list = await _context.Set<Ticketing>().ToListAsync();
            return list;
        }

        public async Task<List<Ticketing>> CurrentUserTickets(int id)
        {
            List<Ticketing> currentUserList = await _context.Set<Ticketing>()
                .Where(x => x.requesterId == id).ToListAsync();
            return currentUserList;
        }
        public async Task<List<Ticketing>> CurrentUserAdminTickets(int id)
        {
            List<Ticketing> currentUserList = await _context.Set<Ticketing>()
                .Where(x => x.specificAssignee == id).ToListAsync();
            return currentUserList;
        }
        /* Single by single statuses selection */
        public async Task<List<Ticketing>> AllTicketsByStatuses(IssueStatuses statuses)
        {
            switch (statuses)
            {
                case IssueStatuses.OPEN:
                    List<Ticketing> openIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.OPEN).ToListAsync();
                    return openIssues;
                case IssueStatuses.INPROGRESS:
                    List<Ticketing> InprogressIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.INPROGRESS)
                        .ToListAsync();
                    return InprogressIssues;
                case IssueStatuses.COMPLETED:
                    List<Ticketing> completed = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.COMPLETED)
                        .ToListAsync();
                    return completed;
                default:
                    List<Ticketing> defaultIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.OPEN).ToListAsync();
                    return defaultIssues;
                
            }
        }

        public async Task<List<Ticketing>> CurrentTicketStatusSearchEngine(int id, IssueStatuses statuses)
        {
            switch (statuses)
            {
                case IssueStatuses.OPEN:
                    List<Ticketing> openIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.OPEN && x.requesterId == id).ToListAsync();
                    return openIssues;
                case IssueStatuses.INPROGRESS:
                    List<Ticketing> InprogressIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.INPROGRESS  && x.requesterId == id)
                        .ToListAsync();
                    return InprogressIssues;
                case IssueStatuses.COMPLETED:
                    List<Ticketing> completed = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.COMPLETED  && x.requesterId == id)
                        .ToListAsync();
                    return completed;
                default:
                    List<Ticketing> defaultIssues = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.OPEN  && x.requesterId == id).ToListAsync();
                    return defaultIssues;
                
            }
        }

        public async Task<IssueStatuses> FindCurrentStatusById(Guid id)
        {
            var status = await _context.Set<Ticketing>()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return status.IssueStatuses;
        }

        public async Task<bool> DeleteTicketById(Guid id)
        {
            var ticket = await _context.Set<Ticketing>()
                .FirstOrDefaultAsync(t => t.Id == id);
            if (ticket == null)
            {
                return false;
            }

            _context.Set<Ticketing>().Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<dynamic> FetchTicketsByPushNotifs()
        {
            var ticketingWithAccounts = await _context.ticketings
                .Where(x => x.IssueStatuses == 0 && x.pushNotif == 1)
                .Join(_context.AccountsEnumerable,
                    ticketing => ticketing.requesterId,
                    account => account.id,
                    (ticketing, account) => new
                    {
                        Ticketing = ticketing,
                        Account = account
                    }).Select(result => new
                    {
                        result.Ticketing.Id,
                        result.Ticketing.ticketId,
                        result.Ticketing.ticketSubject,
                        result.Ticketing.priority,
                        result.Ticketing.description,
                        result.Ticketing.Assignee,
                        result.Ticketing.specificAssignee,
                        result.Ticketing.issue,
                        result.Ticketing.IssueStatuses,
                        result.Ticketing.requester,
                        result.Ticketing.requesterId,
                        result.Ticketing.pc_number,
                        result.Ticketing.comLab,
                        result.Ticketing.pushNotif,
                        result.Ticketing.created_at,
                        result.Ticketing.updated_at,
                        Accounts = new
                        {
                            result.Account.id,
                            result.Account.email,
                            result.Account.username,
                            result.Account.firstname, result.Account.lastname
                        }
                    })
                        .ToListAsync();
            return ticketingWithAccounts;
        }

        public async Task<dynamic> FilteredTicketsFromPushNotification(Guid id)
        {
            var list = await _context.ticketings
                .Where(x => x.Id == id)
                .Join(_context.AccountsEnumerable,
                    ticketing => ticketing.requesterId,
                    account => account.id,
                    (ticketing, account) => new
                    {
                        Ticketing = ticketing,
                        Account = account
                    }).Select(result => new
                    {
                        result.Ticketing.Id,
                        result.Ticketing.ticketId,
                        result.Ticketing.ticketSubject,
                        result.Ticketing.priority,
                        result.Ticketing.description,
                        result.Ticketing.Assignee,
                        result.Ticketing.specificAssignee,
                        result.Ticketing.issue,
                        result.Ticketing.IssueStatuses,
                        result.Ticketing.requester,
                        result.Ticketing.requesterId,
                        result.Ticketing.pc_number,
                        result.Ticketing.comLab,
                        result.Ticketing.pushNotif,
                        result.Ticketing.created_at,
                        result.Ticketing.updated_at,
                        Accounts = new
                        {
                            result.Account.id,
                            result.Account.email,
                            result.Account.username,
                            result.Account.firstname,
                            result.Account.lastname
                        }
                    })
                        .ToListAsync();
            return list;
        }

        public async Task<List<ComLaboratory>> listOfComLabsByGuid(Guid id)
        {
            List<ComLaboratory> list = await _context.Set<ComLaboratory>().Where(x => x.Id == id).ToListAsync();
            return list;
        }

        public async Task<dynamic> AssignToCurrentUser(Guid ticketId, int id)
        {
            var updateCurrentAssignee = await _context.Set<Ticketing>()
                .Where(x => x.Id == ticketId && x.IssueStatuses == 0).FirstOrDefaultAsync();
            if(updateCurrentAssignee != null)
            {
                updateCurrentAssignee.specificAssignee = id;
                await _context.SaveChangesAsync();
                return 200;
            }
            return 0;
        }

        public async Task<dynamic> ChangeStatusFromTicketDetails(Guid ticketId, IssueStatuses status)
        {
            bool findTicketFromDB = await _context.Set<Ticketing>()
                .AnyAsync(x => x.Id == ticketId);
            if (!findTicketFromDB)
            {
                return 403;
            }

            switch(status)
            {
                case IssueStatuses.OPEN:
                    var foundTicketFromDB = await _context.Set<Ticketing>()
                    .Where(x => x.Id == ticketId).FirstOrDefaultAsync();
                    foundTicketFromDB.IssueStatuses = status;
                    foundTicketFromDB.pushNotif = 1;
                    await _context.SaveChangesAsync();
                    return 200;
                case IssueStatuses.INPROGRESS:
                    var foundTicketInProgressFromDB = await _context.Set<Ticketing>()
                    .Where(x => x.Id == ticketId).FirstOrDefaultAsync();
                    foundTicketInProgressFromDB.IssueStatuses = status;
                    foundTicketInProgressFromDB.pushNotif = 2;
                    await _context.SaveChangesAsync();
                    return 200;
                case IssueStatuses.COMPLETED:
                    var foundTicketCompletedFromDB = await _context.Set<Ticketing>()
                    .Where(x => x.Id == ticketId).FirstOrDefaultAsync();
                    foundTicketCompletedFromDB.IssueStatuses = status;
                    foundTicketCompletedFromDB.pushNotif = 3;
                    await _context.SaveChangesAsync();
                    return 200;
            }
            return 403;
        }

        public async Task<dynamic> RemovePC(Guid id)
        {
            var pcInfo = await _context.Set<PC>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (pcInfo != null)
            {
                _context.Set<PC>().Remove(pcInfo);
                await _context.SaveChangesAsync();
                return 200;
            }

            return 400;
        }

        public async Task<dynamic> RemoveComLab(Guid id)
        {
            var comlab = await _context.Set<ComLaboratory>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (comlab != null)
            {
                var pcs = await _context.Set<PC>()
                    .Where(x => x.comlabId == id)
                    .FirstOrDefaultAsync();
                if (pcs != null)
                {
                    _context.Set<PC>().Remove(pcs);
                    _context.Set<ComLaboratory>().Remove(comlab);
                    await _context.SaveChangesAsync();
                    return 200;   
                }
                _context.Set<ComLaboratory>().Remove(comlab);
                await _context.SaveChangesAsync();
                return 200;
            }

            return 400;
        }

        public async Task<dynamic> TotalReports(string type, int? section)
        {
            switch (type)
            {
                case "total-open-tickets":
                    var result = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.OPEN)
                        .CountAsync();
                    return result;
                case "total-inprogress-tickets":
                    var inprogresscount = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.INPROGRESS)
                        .CountAsync();
                    return inprogresscount;
                case "total-completed-tickets":
                    var completedcount = await _context.Set<Ticketing>()
                        .Where(x => x.IssueStatuses == IssueStatuses.COMPLETED)
                        .CountAsync();
                    return completedcount;
                case "total-users":
                    var totaluserscount = await _context.Set<Accounts>()
                        .Where(x => x.status == 1 && x.verified == 1).CountAsync();
                    return totaluserscount;
                case "total-students":
                    var totalStudentscount = await _context.Set<Accounts>()
                        .Where(x => x.access_level == 3 && x.section == section
                                                        && x.status == 1 && x.verified == 1).CountAsync();
                    return totalStudentscount;
                default:
                    return 400;
            }
        }

        class TicketCountByDay
        {
            public DayOfWeek Day { get; set; }
            public int Count { get; set; }
        }
        public async Task<dynamic> AdminReportChartTickets()
        {
            var ticketCounts = await _context.ticketings
                .ToListAsync();

            var filteredCounts = ticketCounts
                .Where(ticket => ticket.created_at.DayOfWeek >= DayOfWeek.Sunday && ticket.created_at.DayOfWeek <= DayOfWeek.Saturday)
                .GroupBy(ticket => ticket.created_at.DayOfWeek)
                .Select(group => new TicketCountByDay
                {
                    Day = group.Key,
                    Count = group.Count()
                })
                .ToList();

            return filteredCounts;
        }

        public async Task<dynamic> FetchPCsUnderComlabId(Guid comlabId)
        {
            bool checkPcByComlabId = await _context.Set<PC>()
                .Where(x => x.comlabId == comlabId)
                .AnyAsync();
            if (checkPcByComlabId)
            {
                var result = await _context.Set<PC>()
                    .Where(x => x.comlabId == comlabId)
                    .ToListAsync();
                return result;
            }
            else
            {
                return 400;
            }
        }


        public async Task<dynamic> FetchTicketsByPushNotifsInProgress()
        {
            var ticketingWithAccounts = await _context.ticketings
                .Where(x => x.IssueStatuses == IssueStatuses.OPEN || x.IssueStatuses == IssueStatuses.INPROGRESS && x.pushNotif == 2 || x.pushNotif == 3)
                .Join(_context.AccountsEnumerable,
                    ticketing => ticketing.requesterId,
                    account => account.id,
                    (ticketing, account) => new
                    {
                        Ticketing = ticketing,
                        Account = account
                    }).Select(result => new
                    {
                        result.Ticketing.Id,
                        result.Ticketing.ticketId,
                        result.Ticketing.ticketSubject,
                        result.Ticketing.priority,
                        result.Ticketing.description,
                        result.Ticketing.Assignee,
                        result.Ticketing.specificAssignee,
                        result.Ticketing.issue,
                        result.Ticketing.IssueStatuses,
                        result.Ticketing.requester,
                        result.Ticketing.requesterId,
                        result.Ticketing.pc_number,
                        result.Ticketing.comLab,
                        result.Ticketing.pushNotif,
                        result.Ticketing.created_at,
                        result.Ticketing.updated_at,
                        Accounts = new
                        {
                            result.Account.id,
                            result.Account.email,
                            result.Account.username,
                            result.Account.firstname,
                            result.Account.lastname
                        }
                    })
                        .ToListAsync();
            return ticketingWithAccounts;
        }

    }
}
