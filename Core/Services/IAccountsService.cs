using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.JWT;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using sti_sys_backend.Utilization.Login;
using Accounts = sti_sys_backend.DataImplementations.Accounts;

namespace sti_sys_backend.Core.Services;

public interface IAccountsService<T> where T: class, Accounts
{
    public Task<Boolean> lookForAccountsToBeSetup();
    public Task<dynamic> AccountSetupCreation(T accounts);
    public Task SendEmailSMTPWithCode(SendEmailHelper sendEmailHelper);
    public JwtSecurityToken CreateToken(List<Claim> claims);
    public String GenerateRefreshToken();
    public Task<dynamic> RefreshToken(AccessWithRefresh accessWithRefresh);
    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    public Task<Boolean> findEmail(AccountSetupHelper accountSetupHelper);
    public Task<dynamic> AccountSigningIn(LoginParams loginParams);
    public Task<dynamic> Revoke(string username);
    public Task<dynamic> CreateModerator(T account);
    public Task<dynamic> CreateStudent(T account);
    public Task<List<T>> ListOfAccounts(List<int> access_levels);
    public Task<dynamic> AccountDeletionToArchive(int id);
    public Task<dynamic> AccountRecoverFromArchive(int id);
    public Task<dynamic> AccountResendOTPRequest(AccountResendOtpParams accountResendOtpParams);
    public Task<List<T>> GetAccountById(int id);
    public Task<List<ProductivityManagement>> findProductivity(int id);
    public Task<dynamic> TimeoutProductivity(int accountId);
    
    /* Edit Profile API */
    public Task<dynamic> CheckCurrentPassword(int uuid, string password);
    public Task<dynamic> ChangeBasicOrPrimaryDetails(ProfileBasicDetails profileBasicDetails);
    public Task<dynamic> RemoveAttachedImageFromDB(int uuid);
    /* Security & Password */
    public Task<dynamic> SecurityChangePassword(SecurityAndPassword securityAndPassword);
    
    /* Student Attendance */
    public Task<dynamic> StudentAttendanceReport(int section);
    public Task<dynamic> CurrentStudentAttendanceReport(int accountId);
    public Task<dynamic> StudentAttendanceReportFilterFromAndTo(DateTime from, DateTime to, int section);
    
    /* student mark statuses */
    public Task<dynamic> StudentMarkStudentStatuses(Guid id, ProductivityStatus productivityStatus);
    public Task<dynamic> CurrentStudentAttendanceReportFilterFromAndTo(DateTime from, DateTime to, int accountId);
    /* Forgot password */
    public Task<dynamic> ForgotPasswordCheckEmailWithOTP(string email);
    public Task<dynamic> ForgotPasswordOTP(OtpFp otpFp);

    public Task<dynamic> ChangePasswordForgotPassword(SecurityAndPasswordFP securityAndPasswordFp);
    
    /* Additional features */
    public Task<dynamic> AdminSideStudentList(int section);
    public Task<dynamic> DisableAccount(int accountId);
    public Task<dynamic> WatchAccountDisabling(int accountId);
    public Task<dynamic> PostActionsLogger(ActionsLogger actionsLogger);
    public Task<dynamic> GetActionsLogger(int accountId);
    public Task<dynamic> GetAccountsDetails(int accountId);
    public Task<bool> IsNewAccount(int id);
    public Task<bool> IsNotVerified(int id);
}