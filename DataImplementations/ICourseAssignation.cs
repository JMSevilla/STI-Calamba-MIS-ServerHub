namespace sti_sys_backend.DataImplementations;

public interface ICourseAssignation
{
    int id { get; set; }
    Guid categoryId { get; set; }
    int courseId { get; set; }
    string description { get; set; }
    Guid subjectId { get; set; }
    string subjectArea { get; set; }
    string subjectName { get; set; }
    int units { get; set; }
    int accountId { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}