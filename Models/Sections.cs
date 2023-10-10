using sti_sys_backend.DataImplementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sti_sys_backend.Models
{
    [Table("sections")]
    public class Sections : ISections
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int section_id { get; set; }
        public string sectionName { get; set; }
        public int status { get; set; }
        public int num_of_students { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string year { get; set; }
        public int course_id { get; set; }
    }
}
