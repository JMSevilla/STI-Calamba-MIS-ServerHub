namespace sti_sys_backend.DataImplementations
{
    public interface IMeetingRoom
    {
        Guid id { get; set; }
        string room_name { get; set; }
        string email { get; set; }
        string room_type { get; set; }
        string? room_description { get; set; }
        string? room_password { get; set; }
        string sectionId { get; set; }
        Guid comlabId { get; set; }
        string room_link { get; set; }
        int room_status { get; set; }
        int room_creator { get; set; }
        int pushNotifs { get; set; }
        DateTime created_at { get; set; }
        DateTime updated_at { get; set; }
    }
}
