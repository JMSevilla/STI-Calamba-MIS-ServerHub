using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("leave_meeting_logs")]
public class LeaveMeeting : ILeaveMeeting
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public Guid roomId { get; set; }
    public int accountId { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public DateTime leaveDate { get; set; }
}