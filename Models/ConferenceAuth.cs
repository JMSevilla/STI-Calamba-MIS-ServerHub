using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("conference_auth")]
public class ConferenceAuth : IConferenceAuth
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public int accountId { get; set; }
    public int isValid { get; set; }
    public DateTime created_at { get; set; }
}