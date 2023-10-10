using sti_sys_backend.Utilization.MailDto;

namespace sti_sys_backend.Utilization.Implementations;

public interface IMailImplementation
{
    Task SendEmailAsync(MailRequest mailRequest);
}