namespace sti_sys_backend.DataImplementations
{
    public interface ICourses
    {
        int id { get; set; }
        string course { get; set; }
        string courseAcronym { get; set; }
        DateTime? createdAt { get; set; }
        DateTime? updatedAt { get; set; }
    }
}
