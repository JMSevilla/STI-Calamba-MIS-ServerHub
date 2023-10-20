using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("signin_approvals")]
public class SigninApproval : ISigninApproval
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public string deviceKey { get; set; }
    public int accountId { get; set; }
    public SigninStatus _signinStatus { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}