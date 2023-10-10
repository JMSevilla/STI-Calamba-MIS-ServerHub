using sti_sys_backend.DataImplementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sti_sys_backend.Models
{
    [Table("courses")]
    public class Courses : ICourses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string course { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public string courseAcronym { get; set; }
    }
}
