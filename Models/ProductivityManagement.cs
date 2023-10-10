using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("productivity_management")]
public class ProductivityManagement : IProductivityManagement
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public int accountId { get; set; }
    public ProductivityStatus _productivityStatus { get; set; }
    public TimeSpan TimeIn { get; set; }
    public TimeSpan TimeOut { get; set; }
    public Status _status { get; set; }
    public DateTime Date { get; set; }
}