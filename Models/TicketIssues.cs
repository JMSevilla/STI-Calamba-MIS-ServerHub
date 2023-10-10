using sti_sys_backend.DataImplementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sti_sys_backend.Models
{
    [Table("ticket_issues")]
    public class TicketIssues : ITicketIssues
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string issue { get; set; }
        public string issueKey { get; set; }
        public int status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
