namespace sti_sys_backend.DataImplementations;

public interface IMailGunSecuredAPIKey
{
    Guid id { get; set; }
    public string domain { get; set; }
    public string key { get; set; }
    public string AuthenticationMechanisms { get; set; }
    ApiStatus _apistatus { get; set; }
}

public enum ApiStatus
{
    ACTIVE,
    INACTIVE
}