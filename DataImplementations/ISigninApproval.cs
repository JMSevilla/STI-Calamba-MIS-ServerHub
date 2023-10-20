namespace sti_sys_backend.DataImplementations;

public interface ISigninApproval
{
    Guid id { get; set; }
    string deviceKey { get; set; }
    int accountId { get; set; }
    SigninStatus _signinStatus { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}

public enum SigninStatus
{
    GRANTED,
    UNAUTHORIZED
}