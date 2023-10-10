using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.AccountsService;

public class AccountsServiceController : AccountsBaseController<Accounts, AccountsConstructor>
{
    public AccountsServiceController(AccountsConstructor repository) : base(repository){}
}