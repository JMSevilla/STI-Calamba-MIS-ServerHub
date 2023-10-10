using sti_sys_backend.DataImplementations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sti_sys_backend.Models
{
    [Table("meeting_room")]
    public class MeetingRoom : IMeetingRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string room_name { get; set; }
        public string room_type { get; set; }
        public int sectionId { get; set; }
        public Guid comlabId { get; set; }
        public string room_link { get; set; }
        public int room_status { get; set; }
        public int room_creator { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string? room_description { get; set; }
        public string? room_password { get; set; }
        public int pushNotifs { get; set; }
        public string email { get; set; }
    }
}
