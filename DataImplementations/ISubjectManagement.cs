namespace sti_sys_backend.DataImplementations;

public interface ISubjectManagement
{
    Guid id { get; set; }
    int courseId { get; set; }
    Guid categoryId { get; set; }
    string subjectName { get; set; }
    string subjectArea { get; set; }
    int units { get; set; }
    string description { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}