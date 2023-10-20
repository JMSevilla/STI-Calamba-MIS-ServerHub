using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("course_management")]
public class CourseManagement : ICourseManagement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int courseId { get; set; }
    public Guid categoryId { get; set; }
    public string courseName { get; set; }
    public string courseAcronym { get; set; }
    public string courseDescription { get; set; }
    public string imgurl { get; set; }
    public int numbersOfStudent { get; set; }
    public int maximumStudents { get; set; }
    public CourseSlotStatus _courseSlotStatus { get; set; }
    public CourseStatus _courseStatus { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}