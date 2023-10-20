namespace sti_sys_backend.DataImplementations;

public interface ISectionAssignation
{
    int id { get; set; }
    int accountId { get; set; }
    int sectionId { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}