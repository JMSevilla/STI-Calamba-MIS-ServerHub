using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("subject_assignation")]
public class SubjectAssignation : ISubjectAssignation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public Guid subjectId { get; set; }
    public int accountId { get; set; }
    public int courseId { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}