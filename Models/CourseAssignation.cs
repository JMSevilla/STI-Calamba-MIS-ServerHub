using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("course_assignation")]
public class CourseAssignation : ICourseAssignation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public Guid categoryId { get; set; }
    public int courseId { get; set; }
    public string description { get; set; }
    public Guid subjectId { get; set; }
    public string subjectArea { get; set; }
    public string subjectName { get; set; }
    public int units { get; set; }
    public int accountId { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}