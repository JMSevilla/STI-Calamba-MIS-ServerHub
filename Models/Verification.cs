using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("verification")]
public class Verification : IVerification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }
    public int code { get; set; }
    public int resendCount { get; set; }
    public int isValid { get; set; }
    public string? type { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}