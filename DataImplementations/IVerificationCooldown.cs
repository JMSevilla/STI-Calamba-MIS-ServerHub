namespace sti_sys_backend.DataImplementations;

public interface IVerificationCooldown
{
    int id { get; set; }
    int resendCount { get; set; }
    long cooldown { get; set; }
}