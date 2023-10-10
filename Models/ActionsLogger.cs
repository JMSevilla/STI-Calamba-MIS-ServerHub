using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;
[Table("actions_logger")]
public class ActionsLogger : IActionsLogger
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public int accountId { get; set; }
    public string actionsMessage { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}