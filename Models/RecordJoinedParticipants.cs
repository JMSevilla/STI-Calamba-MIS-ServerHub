using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("record_joined_participants")]
public class RecordJoinedParticipants : IRecordJoinedParticipants
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int accountId { get; set; }
    public Guid room_id { get; set; }
    public Guid comlabId { get; set; }
    public RecordJoinedStatus _RecordJoinedStatus { get; set; }
    public DateTime date_joined { get; set; }
    public DateTime date_left { get; set; }
}