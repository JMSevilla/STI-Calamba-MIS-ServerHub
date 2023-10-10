using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("joined_participants")]
public class JoinedParticipants : IJoinedParticipants
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int accountId { get; set; }
    public Guid room_id { get; set; }
    public Guid comlabId { get; set; }
    public JoinedStatus _joinedStatus { get; set; }
    public DateTime date_joined { get; set; }
    public DateTime date_left { get; set; }
}