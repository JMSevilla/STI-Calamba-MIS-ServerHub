using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("jitsi_private_key")]
public class JitsiPrivateKey: JitsiPrivateKeyStorage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    [Required]
    public byte[] PrivateKey { get; set; }
    public int active { get; set; }
}