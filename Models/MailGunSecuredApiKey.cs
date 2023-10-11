using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("mail_gun_secured")]
public class MailGunSecuredApiKey : IMailGunSecuredAPIKey
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public string domain { get; set; }
    public string key { get; set; }
    public ApiStatus _apistatus { get; set; }
}