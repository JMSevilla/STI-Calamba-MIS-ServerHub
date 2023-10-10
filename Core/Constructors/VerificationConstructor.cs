using Microsoft.Extensions.Options;
using sti_sys_backend.Core.ServiceImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization.MailDto;

namespace sti_sys_backend.Core.Constructors;

public class VerificationConstructor : VerificationWithCooldownImpl<Verification, DatabaseQueryable>
{
    public VerificationConstructor(DatabaseQueryable context, IOptions<MailSettings> mailSettings) : base(context, mailSettings) {}
}