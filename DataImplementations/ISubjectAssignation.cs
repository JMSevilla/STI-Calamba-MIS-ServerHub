namespace sti_sys_backend.DataImplementations;

public interface ISubjectAssignation
{
    Guid id { get; set; }
    Guid subjectId { get; set; }
    int accountId { get; set; }
    int courseId { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}