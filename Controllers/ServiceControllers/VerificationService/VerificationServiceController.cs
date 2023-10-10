using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Controllers.BaseControllers.VerificationBase;
using sti_sys_backend.Core.Constructors;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.ServiceControllers.VerificationService;

public class VerificationServiceController : VerificationBaseController<Verification, VerificationConstructor>
{
    public VerificationServiceController(VerificationConstructor verificationConstructor) : base(verificationConstructor) {}
}