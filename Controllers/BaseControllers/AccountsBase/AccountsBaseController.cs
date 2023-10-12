using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sti_sys_backend.Authentication;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.JWT;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.Login;

namespace sti_sys_backend.Controllers.BaseControllers;

[Route("api/v1/[controller]")]
[ApiController]
[ServiceFilter(typeof(KeyAuthFilter))]
public abstract class AccountsBaseController<TEntity, TRepository> : ControllerBase
    where TEntity : class, DataImplementations.Accounts
    where TRepository : IAccountsService<TEntity>
{
    private readonly TRepository _repository;

    public AccountsBaseController(TRepository repository)
    {
        _repository = repository;
    }

    [Route("find-any-accounts"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> FindAnyAccounts()
    {
        bool result = await _repository.lookForAccountsToBeSetup();
        return Ok(result);
    }

    [Route("map-primary-check"), HttpGet, HttpPut, HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> MapEmails([FromBody] AccountSetupHelper accountSetupHelper)
    {
        bool result = (await _repository.findEmail(accountSetupHelper));
        return Ok(result);
    }

    [Route("account-creation-activation"), HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AccountCreationActivation([FromBody] TEntity entity)
    {
        var result = (await _repository.AccountSetupCreation(entity));
        return Ok(result);
    }

    [Route("account-login"), HttpPost, HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginParams loginParams)
    {
        var result = (await _repository.AccountSigningIn(loginParams));
        return Ok(result);
    }

    [Route("refresh-token"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] AccessWithRefresh accessWithRefresh)
    {
        var result = (await _repository.RefreshToken(accessWithRefresh));
        return new ObjectResult(result);
    }

    [Route("revoke-token/{username}"), HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> RevokeToken([FromRoute] string username)
    {
        var result = (await _repository.Revoke(username));
        return Ok(result);
    }

    [Route("create-moderator"), HttpPost, HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(403)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ModeratorCreation(TEntity entity)
    {
        var result = (await _repository.CreateModerator(entity));
        return Ok(result);
    }

    [Route("list-accounts"), HttpGet, HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.Accounts>))]
    public async Task<IActionResult> ListOfAccounts([FromBody] List<int> access_levels)
    {
        List<TEntity> accounts = (await _repository.ListOfAccounts(access_levels));
        return Ok(accounts);
    }

    [Route("account-disabling/{id}"), HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(403)]
    public async Task<IActionResult> AccountDisabling([FromRoute] int id)
    {
        var result = (await _repository.AccountDeletionToArchive(id));
        return Ok(result);
    }

    [Route("account-recover-from-archive/{id}"), HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AccountRecoveryFromArchive([FromRoute] int id)
    {
        var result = (await _repository.AccountRecoverFromArchive(id));
        return Ok(result);
    }
    
    [Route("account-resend-otp"), HttpPost, HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AccountResendOtp([FromBody] AccountResendOtpParams accountResendOtpParams)
    {
        var result = (await _repository.AccountResendOTPRequest(accountResendOtpParams));
        return Ok(result);
    }

    [Route("create-student"), HttpPost, HttpPut]
    [AllowAnonymous]
    [ProducesResponseType(403)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> createStudent(TEntity entity)
    {
        var result = (await _repository.CreateStudent(entity));
        return Ok(result);
    }

    [Route("find-account-by-id/{id}"), HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.Accounts>))]
    public async Task<IActionResult> FindAccountById([FromRoute] int id)
    {
        List<TEntity> list = (await _repository.GetAccountById(id));
        return Ok(list);
    }

    [Route("find-productivity-student/{id}"), HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.ProductivityManagement>))]
    public async Task<IActionResult> findProductivity([FromRoute] int id)
    {
        var list = (await _repository.findProductivity(id));
        return Ok(list);
    }

    [Route("logout-with-timeout/{accountId}"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> LogoutWithTimeOut([FromRoute] int accountId)
    {
        var res = await _repository.TimeoutProductivity(accountId);
        return Ok(res);
    }

    [Route("check-current-password/{uuid}/{password}"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CheckCurrentPassword([FromRoute] int uuid, [FromRoute] string password)
    {
        var result = await _repository.CheckCurrentPassword(uuid, password);
        return Ok(result);
    }

    [Route("change-basic-or-primary"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeBasicOrPrimaryDetails([FromBody] ProfileBasicDetails profileBasicDetails)
    {
        var result = await _repository.ChangeBasicOrPrimaryDetails(profileBasicDetails);
        return Ok(result);
    }

    [Route("remove-attached-image/{uuid}"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> RemoveAttachedImage([FromRoute] int uuid)
    {
        var result = await _repository.RemoveAttachedImageFromDB(uuid);
        return Ok(result);
    }

    [Route("security-change-password"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> SecurityChangePassword([FromBody] SecurityAndPassword securityAndPassword)
    {
        var result = await _repository.SecurityChangePassword(securityAndPassword);
        return Ok(result);
    }

    [Route("student-attendance-initialized/{section}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> StudentAttendanceInitialized([FromRoute] int section)
    {
        var result = await _repository.StudentAttendanceReport(section);
        return Ok(result);
    }
    
    [Route("current-student-attendance-initialized/{accountId}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CurrentStudentAttendanceInitialized([FromRoute] int accountId)
    {
        var result = await _repository.CurrentStudentAttendanceReport(accountId);
        return Ok(result);
    }

    [Route("student-attendance-filtering/from/{from}/to/{to}/{section}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> StudentReportFiltering([FromRoute] DateTime from, [FromRoute] DateTime to,
        [FromRoute] int section)
    {
        var result = await _repository.StudentAttendanceReportFilterFromAndTo(from, to, section);
        return Ok(result);
    }

    [Route("mark-student-statuses/{id}/{productivityStatus}"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> MarkStudentStatuses([FromRoute] Guid id,
        [FromRoute] ProductivityStatus productivityStatus)
    {
        var result = await _repository.StudentMarkStudentStatuses(id, productivityStatus);
        return Ok(result);
    }
    
    [Route("current-student-attendance-filtering/from/{from}/to/{to}/{accountId}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> CurrentStudentReportFiltering([FromRoute] DateTime from, [FromRoute] DateTime to,
        [FromRoute] int accountId)
    {
        var result = await _repository.CurrentStudentAttendanceReportFilterFromAndTo(from, to, accountId);
        return Ok(result);
    }

    [Route("forgot-password-email-with-otp"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordWithOTP([FromBody] string email)
    {
        var result = await _repository.ForgotPasswordCheckEmailWithOTP(email);
        return Ok(result);
    }

    [Route("forgot-password-otp"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordOTP([FromBody] OtpFp otpFp)
    {
        var result = await _repository.ForgotPasswordOTP(otpFp);
        return Ok(result);
    }

    [Route("change-password-fp"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePasswordFP([FromBody] SecurityAndPasswordFP securityAndPasswordFp)
    {
        var result = await _repository.ChangePasswordForgotPassword(securityAndPasswordFp);
        return Ok(result);
    }

    [Route("student-list-admin-side/{section}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> StudentListAdminSide([FromRoute] int section)
    {
        var result = await _repository.AdminSideStudentList(section);
        return Ok(result);
    }

    [Route("disable-account/{accountId}"), HttpPut]
    [AllowAnonymous]
    public async Task<IActionResult> DisableAccount([FromRoute] int accountId)
    {
        var result = await _repository.DisableAccount(accountId);
        return Ok(result);
    }

    [Route("watch-disabled-account/{accountId}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> WatchAccountDisabled([FromRoute] int accountId)
    {
        bool result = await _repository.WatchAccountDisabling(accountId);
        return Ok(result);
    }

    [Route("actions-logger"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ActionsLogger([FromBody] ActionsLogger actionsLogger)
    {
        var result = await _repository.PostActionsLogger(actionsLogger);
        return Ok(result);
    }

    [Route("get-account-details/{accountId}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccountsDetails([FromRoute] int accountId)
    {
        var result = await _repository.GetAccountsDetails(accountId);
        return Ok(result);
    }

    [Route("get-actions-logger/{id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActionsLogger([FromRoute] int id)
    {
        var result = await _repository.GetActionsLogger(id);
        return Ok(result);
    }

    [Route("detect-new-account/{id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> DetectNewAccount([FromRoute] int id)
    {
        bool result = await _repository.IsNewAccount(id);
        return Ok(result);
    }

    [Route("detect-account-unverified/{id}"), HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> DetectAccountUnverified([FromRoute] int id)
    {
        bool result = await _repository.IsNotVerified(id);
        return Ok(result);
    }

    [Route("email-test-send"), HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SendSMTP([FromBody] SendEmailHelper sendEmailHelper)
    {
        var result = await _repository.SendEmailSMTPWithCode(sendEmailHelper);
        return Ok(result);
    }
}