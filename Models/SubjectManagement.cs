using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("subject_management")]
public class SubjectManagement : ISubjectManagement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }

    public int courseId { get; set; }
    public Guid categoryId { get; set; }
    public string subjectName { get; set; }
    public string subjectArea { get; set; }
    public int units { get; set; }
    public string description { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}