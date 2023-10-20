using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("section_assignation")]
public class SectionAssignation : ISectionAssignation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int accountId { get; set; }
    public int sectionId { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}