using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("accounts")]
public class Accounts : DataImplementations.Accounts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string email { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string username { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string password { get; set; }
    [DataType(DataType.Text)]
    public string firstname { get; set; }
    [DataType(DataType.Text)]
    public string middlename { get; set; }
    [DataType(DataType.Text)]
    public string lastname { get; set; }
    [DataType(DataType.Text)]
    public string mobileNumber { get; set; }
    public string? imgurl { get; set; }
    public int status { get; set; }
    public int verified { get; set; }
    public int access_level { get; set; }
    public int section { get; set; }
    public string multipleSections { get; set; }
    public int isNewAccount { get; set; }
    public int invalidCount { get; set; }
    public int isArchived { get; set; }
    public ActiveStatus _activeStatus { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public int course_id { get; set; }
}