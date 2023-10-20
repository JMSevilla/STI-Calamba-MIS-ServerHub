namespace sti_sys_backend.DataImplementations;

public interface ICourseManagement
{
    int id { get; set; }
    int courseId { get; set; }
    Guid categoryId { get; set; }
    string courseName { get; set; }
    string courseAcronym { get; set; }
    string courseDescription { get; set; }
    string imgurl { get; set; }
    int numbersOfStudent { get; set; }
    int maximumStudents { get; set; }
    CourseSlotStatus _courseSlotStatus { get; set; }
    CourseStatus _courseStatus { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}

public enum CourseSlotStatus
{
    FULL_SLOT,
    HAS_AVAILABLE_SLOT
}

public enum CourseStatus
{
    ACTIVE,
    INACTIVE
}