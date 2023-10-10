using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;

namespace sti_sys_backend.Core.Services;

public interface IVerificationService<T> where T: class, IVerification
{
    public Task<dynamic> CheckVerificationCountsWhenLoad(string email);
    public Task<dynamic> PostNewVerificationCooldowns(VerificationCooldown verificationCooldown);
    public Task<dynamic> ResendVerificationCode(string type, string email);
    public Task SendEmailSMTPWithCode(string email, int code, string? body);
    public Task<dynamic> CheckVerificationCode(int code, string email, string? type = "account_activation");
}