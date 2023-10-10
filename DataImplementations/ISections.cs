namespace sti_sys_backend.DataImplementations
{
    public interface ISections
    {
        int id { get; set; }
        int section_id { get; set; }
        string sectionName { get; set; }
        int status { get; set; }
        int course_id { get; set; }
        string year { get; set; }
        int num_of_students { get; set; }
        DateTime created_at { get; set; }
        DateTime updated_at { get; set; }
    }
}
