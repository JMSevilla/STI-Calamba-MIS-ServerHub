namespace sti_sys_backend.DataImplementations;

public interface IDeviceRecognition
{
    Guid id { get; set; }
    string deviceKey { get; set; }
    int accountId { get; set; }
    int signinRequest { get; set; }
    int rejects { get; set; }
    int approved { get; set; }
    AppGranted _appGranted { get; set; }
    DateTime created_at { get; set; }
    DateTime updated_at { get; set; }
}

public enum AppGranted
{
    ACTIVE,
    INACTIVE
}