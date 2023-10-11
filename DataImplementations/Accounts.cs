namespace sti_sys_backend.DataImplementations;

public interface Accounts
{
    int id { get; set; }
    string email { get; set; }
    string username { get; set; }
    string password { get; set; }
    string firstname { get; set; }
    string middlename { get; set; }
    string lastname { get; set; }
    string mobileNumber { get; set; }
    string? imgurl { get; set; }
    int status { get; set; }
    int verified { get; set; }
    int access_level { get; set; }
    int section { get; set; }
    int course_id { get; set; }
    int isNewAccount { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}