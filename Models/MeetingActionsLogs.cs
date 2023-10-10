using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("meeting_actions_logs")]
public class MeetingActionsLogs : IMeetingActionsLogs
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    private IMeetingActionsLogs _meetingActionsLogsImplementation;
    public Guid id { get; set; }
    public string log_message { get; set; }
    public int accountId { get; set; }
    public Guid room_id { get; set; }
    public int? violations { get; set; }
    public MeetingAuthorization? _meetingAuthorization { get; set; }
    public DateTime? logDateTime { get; set; }
}