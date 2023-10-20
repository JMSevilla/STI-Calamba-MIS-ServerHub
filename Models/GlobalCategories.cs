using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using sti_sys_backend.DataImplementations;

namespace sti_sys_backend.Models;

[Table("global_categories")]
public class GlobalCategories : IGlobalCategories
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid id { get; set; }
    public string categoryName { get; set; }
    public string categoryDescription { get; set; }
    public string categoryPath { get; set; }
    public CategoryStatus _categoryStatus { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}
