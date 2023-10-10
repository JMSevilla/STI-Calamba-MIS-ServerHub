
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models
{
    [Table("ticketing")]
    public class Ticketing : ITicketing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string ticketId { get; set; }
        public string ticketSubject { get; set; }
        public string priority { get; set; }
        public string? description { get; set; }
        public string Assignee { get; set; }
        public int specificAssignee { get; set; }
        public string issue { get; set; }
        public IssueStatuses IssueStatuses { get; set; }
        public string requester { get; set; }
        public int requesterId { get; set; }
        public string pc_number { get; set; }
        public string comLab { get; set; }
        public int pushNotif { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
