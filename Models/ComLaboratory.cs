using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("com_laboratory")]
public class ComLaboratory: IComLaboratory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string comlabName { get; set; }
    public int totalComputers { get; set; }
    public int totalWorkingComputers { get; set; }
    public int totalNotWorkingComputers { get; set; }
    public int totalNoNetworkComputers { get; set; }
}