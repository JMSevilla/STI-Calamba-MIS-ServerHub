using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.Models;

namespace sti_sys_backend.Controllers.BaseControllers.VerificationBase;

[Route("api/v1/[controller]")]
[ApiController]
[ServiceFilter(typeof(KeyAuthFilter))]
public abstract class VerificationBaseController<TEntity, TRepository> : ControllerBase
    where TEntity: class, IVerification
    where TRepository: IVerificationService<TEntity>
{
    private readonly TRepository _repository;

    public VerificationBaseController(TRepository repository)
    {
        _repository = repository;
    }

    [Route("create-new-verification-cooldowns"), HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateNewVCCooldowns([FromBody] VerificationCooldown verificationCooldown)
    {
        var result = (await _repository.PostNewVerificationCooldowns(verificationCooldown));
        return Ok(result);
    }

    [Route("resend-verification-code/{type}/account/{email}"), HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ResendNewVerificationCode([FromRoute] string type, [FromRoute] string email)
    {
        var result = (await _repository.ResendVerificationCode(type, email));
        return Ok(result);
    }

    [Route("code-entry/{code}/account/{email}/type/{type}"), HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CodeEntry([FromRoute] int code, [FromRoute] string email, [FromRoute] string? type)
    {
        var result = (await _repository.CheckVerificationCode(code, email, type));
        return Ok(result);
    }

    [Route("check-resend-code/{email}"), HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CheckResendCode([FromRoute] string email)
    {
        var result = (await _repository.CheckVerificationCountsWhenLoad(email));
        return Ok(result);
    }
}