using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using sti_sys_backend.Core.ServiceImplementations;
using sti_sys_backend.JWT;
using sti_sys_backend.Models;
using sti_sys_backend.DB;
using sti_sys_backend.Utilization.MailDto;

namespace sti_sys_backend.Core.Constructors;

public class AccountsConstructor : AccountsImpl<Accounts, DatabaseQueryable>
{
    public AccountsConstructor(DatabaseQueryable context, IOptions<MailSettings> mailSettings, UserManager<JWTIdentity> userManager, RoleManager<IdentityRole> roleManager,
         IConfiguration configuration) : base(context, mailSettings, userManager, roleManager, configuration){}
}