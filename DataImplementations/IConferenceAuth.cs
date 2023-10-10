namespace sti_sys_backend.DataImplementations;

public interface IConferenceAuth
{
    int id { get; set; }
    string firstname { get; set; }
    string lastname { get; set; }
    string access_token { get; set; }
    string refresh_token { get; set; }
    int accountId { get; set; }
    int isValid { get; set; }
    DateTime created_at { get; set; }
}