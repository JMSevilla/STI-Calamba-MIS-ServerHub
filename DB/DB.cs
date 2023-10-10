using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using sti_sys_backend.JWT;
using sti_sys_backend.Models;

namespace sti_sys_backend.DB;

public class DatabaseQueryable : IdentityDbContext<JWTIdentity>
{
    public DatabaseQueryable(DbContextOptions<DatabaseQueryable> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
    public DbSet<Accounts> AccountsEnumerable { get; set; }
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<VerificationCooldown> VerificationCooldowns { get; set; }
    public DbSet<Sections> SectionsEnumerable { get; set; }
    public DbSet<Courses> CoursesEnumerable { get; set; }
    public DbSet<TicketIssues> ticketIssues { get; set; }
    public DbSet<Ticketing> ticketings { get; set; }
    public DbSet<PC> Pcs { get; set; }
    public DbSet<ComLaboratory> ComLaboratories { get; set; }
    public DbSet<MeetingRoom> MeetingRooms { get; set; }
    public DbSet<ProductivityManagement> ProductivityManagements { get; set; }
    public DbSet<LeaveMeeting> LeaveMeetings { get; set; }
    public DbSet<ConferenceAuth> ConferenceAuths { get; set; }
    public DbSet<JoinedParticipants> JoinedParticipantsEnumerable { get; set; }
    public DbSet<MeetingActionsLogs> MeetingActionsLogsEnumerable { get; set; }
    public DbSet<Settings> SettingsEnumerable { get; set; }
    public DbSet<RecordJoinedParticipants> RecordJoinedParticipantsEnumerable { get; set; }
    public DbSet<ActionsLogger> ActionsLoggers { get; set; }
}