namespace sti_sys_backend.Utilization;

public class AccountsHelper
{
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string firstname { get; set; }
    public string middlename { get; set; }
    public string lastname { get; set; }
    public string mobileNumber { get; set; }
    public string? imgurl { get; set; }
    public int status { get; set; }
    public int verified { get; set; }
    public int access_level { get; set; }
    public int section { get; set; }
    public int course_id { get; set; }
    public int isNewAccount { get; set; }
    public int type { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}