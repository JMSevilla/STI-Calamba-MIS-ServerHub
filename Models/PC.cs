using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("pc")]
public class PC : IPC
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public OperatingSys operatingSystem { get; set; }
    public string computerName { get; set; }
    public Guid comlabId { get; set; }
    public ComputerStatus computerStatus { get; set; }
}