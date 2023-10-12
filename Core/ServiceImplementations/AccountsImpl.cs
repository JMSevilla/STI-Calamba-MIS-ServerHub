using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DB;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using sti_sys_backend.JWT;
using sti_sys_backend.Utilization;
using sti_sys_backend.Models;
using System;
using System.Linq.Dynamic.Core;
using sti_sys_backend.Utilization.Login;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using sti_sys_backend.DataImplementations;
using JsonSerializer = System.Text.Json.JsonSerializer;
using RestSharp;
using RestSharp.Authenticators;
using SendGrid;
using SendGrid.Helpers.Mail;
using MailSettings = sti_sys_backend.Utilization.MailDto.MailSettings;

namespace sti_sys_backend.Core.ServiceImplementations;

public abstract class AccountsImpl<TEntity, TContext> : IAccountsService<TEntity>
    where TEntity : class, DataImplementations.Accounts
    where TContext : DatabaseQueryable
{
    private readonly TContext context;
    private readonly MailSettings _mailSettings;
    private readonly UserManager<JWTIdentity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private IConfiguration _configuration;
    public AccountsImpl(
        TContext context,
        IOptions<MailSettings> mailSettings,
        UserManager<JWTIdentity> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        this.context = context;
        _mailSettings = mailSettings.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }
    public async Task<bool> lookForAccountsToBeSetup()
    {
        bool found = await context.AccountsEnumerable.AnyAsync();
        return found;
    }
    public async Task<dynamic> CreateModerator(TEntity account)
    {
        bool foundAccountModerator = await context.Set<TEntity>()
            .AnyAsync(x => x.username == account.username || x.email == account.email && x.access_level == 2);
        var accountExists = await _userManager.FindByEmailAsync(account.email);
        if(foundAccountModerator)
        {
            return 403;
        } 
        else
        {
            if (accountExists != null)
                return 403;
            JWTIdentity user = new()
            {
                Email = account.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = account.username,
            };
            Verification verification = new Verification();
            var isThereAnyVerificationsFromDB = await context.Set<Verification>()
                .AnyAsync(x => x.email == account.email && x.isValid == 1);
            
            if (isThereAnyVerificationsFromDB)
            {
                int code = int.Parse(GenerateVerificationCode.GenerateCode());
                var whenVerificationsExists = await context.Set<Verification>()
                .Where(x => x.email == account.email && x.isValid == 1).FirstOrDefaultAsync();
                
                whenVerificationsExists.resendCount = whenVerificationsExists.resendCount + 1;
                whenVerificationsExists.code = code;
                var result = await _userManager.CreateAsync(user, account.password);
                if (!result.Succeeded)
                    return "password_too_weak";
                
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.password);
                account.password = hashedPassword;
                account.access_level = 2;
                account.section = account.section;
                account.course_id = account.course_id;
                account.status = 1;
                account.verified = 0;
                account.isNewAccount = 1;
                account.mobileNumber = account.mobileNumber;
                account.imgurl = "no-image-attached";
                account.created_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                account.updated_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                await context.Set<TEntity>().AddAsync(account);
                await context.SaveChangesAsync();
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = account.email,
                    code = code,
                    body = "This is your activation code"
                });
                return 200;
            }
            else
            {
                int code = int.Parse(GenerateVerificationCode.GenerateCode());
                var result = await _userManager.CreateAsync(user, account.password);
                if (!result.Succeeded)
                    return "password_too_weak";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.password);
                account.password = hashedPassword;
                account.access_level = 2;
                account.section = account.section;
                account.course_id = account.course_id;
                account.status = 1;
                account.verified = 0;
                account.isNewAccount = 1;
                account.mobileNumber = account.mobileNumber;
                account.imgurl = "no-image-attached";
                account.created_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                account.updated_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                verification.email = account.email;
                verification.code = code;
                verification.resendCount = 1;
                verification.isValid = 1;
                verification.type = "email";
                verification.createdAt = DateTime.Now;
                verification.updatedAt = DateTime.Now;
                await context.Set<Verification>().AddAsync(verification);
                await context.Set<TEntity>().AddAsync(account);
                await context.SaveChangesAsync();
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = account.email,
                    code = code,
                    body = "This is your activation code"
                });
                return 200;
            }
        }
    }
    public async Task<dynamic> AccountSetupCreation(TEntity accounts)
    {
        bool foundExistingAccount = await context.Set<TEntity>().AnyAsync(x => x.username == accounts.username
                                                                               || x.email == accounts.email &&
                                                                               x.status == 1);
        var accountExists = await _userManager.FindByEmailAsync(accounts.email);
        
        if (foundExistingAccount)
        {
            return 403;
        }
        else
        {
            if (accountExists != null)
                return "auth_account_exists";
            JWTIdentity user = new()
            {
                Email = accounts.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = accounts.username
            };

            var result = await _userManager.CreateAsync(user, accounts.password);
            if (!result.Succeeded)
                return "password_too_weak";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(accounts.password);
            accounts.password = hashedPassword;
            accounts.access_level = 1;
            accounts.section = 0;
            accounts.course_id = 0;
            accounts.status = 1;
            accounts.verified = 1;
            accounts.mobileNumber = accounts.mobileNumber;
            accounts.imgurl = "no-image-attached";
            accounts.created_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            accounts.updated_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
            await context.Set<TEntity>().AddAsync(accounts);
            await context.SaveChangesAsync();
            return 200;
        }
    }

    public async Task<dynamic> SendEmailSMTPWithCode(SendEmailHelper sendEmailHelper)
    {
        /*string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\emailTemplate.html";
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();
        MailText = MailText.Replace("[username]", "User").Replace("[email]", email).Replace("[verificationCode]", Convert.ToString(code))
            .Replace("[body]", body);
        var mail = new MimeMessage();
        mail.Sender = MailboxAddress.Parse(_mailSettings.Mail);
        mail.To.Add(MailboxAddress.Parse(email));
        mail.Subject = $"Welcome {email}";
        var builder = new BodyBuilder();
        builder.HtmlBody = MailText;
        mail.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(mail);
        smtp.Disconnect(true);*/
        /*var rGetKey = await context.Set<MailGunSecuredApiKey>()
            .Where(x => x._apistatus == ApiStatus.ACTIVE)
            .FirstOrDefaultAsync();
        string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\emailTemplate.html";
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();
        MailText = MailText.Replace("[username]", "User").Replace("[email]", email).Replace("[verificationCode]", Convert.ToString(code))
            .Replace("[body]", body);
        var mail = new MimeMessage();
        var builder = new BodyBuilder();
        mail.From.Add(new MailboxAddress("System Administrator", "devopsbyte60@" + rGetKey.domain));
        mail.To.Add(new MailboxAddress("User", email));
        builder.HtmlBody = MailText;
        mail.Subject = $"Welcome {email}";
        mail.Body = builder.ToMessageBody();
        using (var client = new SmtpClient())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect("smtp.mailgun.org", 587, false);
            client.AuthenticationMechanisms.Remove(rGetKey.AuthenticationMechanisms);
            client.Authenticate("postmaster@" + rGetKey.domain, rGetKey.key);
            client.Send(mail);
            client.Disconnect(true);
        }*/
        // Create a RestClient with the base URL
        try
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\emailTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", "User").Replace("[email]", sendEmailHelper.email).Replace("[verificationCode]", Convert.ToString(sendEmailHelper.code))
                .Replace("[body]", sendEmailHelper.body);
            var mail = new MimeMessage();
            mail.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            mail.To.Add(MailboxAddress.Parse(sendEmailHelper.email));
            mail.Subject = $"Welcome {sendEmailHelper.email}";
            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            mail.Body = builder.ToMessageBody();
            var rGetKey = await context.Set<MailGunSecuredApiKey>()
                .Where(x => x._apistatus == ApiStatus.ACTIVE)
                .FirstOrDefaultAsync();
            var apiKey = rGetKey.domain;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("lizkiethbabael@gmail.com", "STI System");
            var subject = "STI System Notification";
            var to = new EmailAddress(sendEmailHelper.email, "User");
            var plainTextContent = "STI SYSTEM NOTIFICATIONS";
            var htmlContent = MailText;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var result = await client.SendEmailAsync(msg);
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public JwtSecurityToken CreateToken(List<Claim> claims)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"]));
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<dynamic> RefreshToken(AccessWithRefresh accessWithRefresh)
    {
        dynamic obj = new ExpandoObject();
        if (accessWithRefresh is null)
        {
            return "Invalid client request";
        }

        string? accessToken = accessWithRefresh.AccessToken;
        string? refreshToken = accessWithRefresh.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return "Invalid access token or refresh token";
        }

        string username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return "Invalid access token or refresh token";
        }

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        obj.accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
        obj.refreshToken = newRefreshToken;
        return obj;
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:IssuerSigningKey"])),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    public async Task<bool> findEmail(AccountSetupHelper accountSetupHelper)
    {
        Verification verification = new Verification();
        int code = int.Parse(GenerateVerificationCode.GenerateCode());
        bool existingVrf = await context.Set<Verification>().AnyAsync(x => x.email == accountSetupHelper.email && x.isValid == 1);
        bool findEmailOrUsernameExists = await context.Set<TEntity>().AnyAsync(x => x.username == accountSetupHelper.username || x.email == accountSetupHelper.email);
        if(findEmailOrUsernameExists)
        {
            return findEmailOrUsernameExists;
        }
        else
        {
            if (existingVrf)
            {
                var vrfToBeUpdate = await context.Set<Verification>().Where(x => x.email == accountSetupHelper.email && x.isValid == 1).FirstOrDefaultAsync();
                vrfToBeUpdate.resendCount = vrfToBeUpdate.resendCount + 1;
                vrfToBeUpdate.code = code;
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = accountSetupHelper.email,
                    code = code,
                    body = "STI Monitoring System Account Activation Code"
                });
                await context.SaveChangesAsync();
                return findEmailOrUsernameExists;
            }
            else
            {
                verification.code = code;
                verification.email = accountSetupHelper.email;
                verification.resendCount = 1;
                verification.isValid = 1;
                verification.type = "email";
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = accountSetupHelper.email,
                    code = code,
                    body = "STI Monitoring System Account Activation Code"
                });
                await context.AddAsync(verification);
                await context.SaveChangesAsync();
                return findEmailOrUsernameExists;
            }
        }
    }

    class WorldTimeApiResponse
    {
        public string dateTime { get; set; }
        public long unixtime { get; set; }
    }
    public async Task<dynamic> AccountSigningIn(LoginParams loginParams)
    {
        string apiUrl = "https://timeapi.io/api/Time/current/zone?timeZone=Asia/Manila";
        var accounts = await _userManager.FindByNameAsync(loginParams.username);
        if(accounts == null)
        {
            return "ACCOUNT_NOT_FOUND_ON_THIS_SECTION";
        }
        var findSectionIdByUsername = await context.Set<TEntity>().Where(x => x.username == loginParams.username).FirstOrDefaultAsync();
        var findAllAccountsDetails = await context.Set<TEntity>().Where(x => x.username == loginParams.username && x.section == findSectionIdByUsername.section).FirstOrDefaultAsync();
        bool isAnyAccountFromSource = await context.Set<TEntity>().AnyAsync(x => x.username == loginParams.username && x.status == 1);
        dynamic expandedObj = new ExpandoObject();
        if(string.IsNullOrWhiteSpace(loginParams.username) || string.IsNullOrWhiteSpace(loginParams.password)) {
            return "required_fields_empty";
        }

        string encryptedPassword = findAllAccountsDetails == null ? "" : findAllAccountsDetails.password;
        if(isAnyAccountFromSource)
        {
            if (BCrypt.Net.BCrypt.Verify(loginParams.password, encryptedPassword))
            {
                if (findAllAccountsDetails.section == findSectionIdByUsername.section)
                {
                    if (accounts != null && await _userManager.CheckPasswordAsync(accounts, loginParams.password))
                    {
                        var accountsRoles = await _userManager.GetRolesAsync(accounts);
                        var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, accounts.UserName),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                                };
                        foreach (var accountRole in accountsRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, accountRole));
                        }
                        var token = CreateToken(claims);
                        var refreshToken = GenerateRefreshToken();

                        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                        accounts.RefreshToken = refreshToken;
                        accounts.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                        await _userManager.UpdateAsync(accounts);
                        if (findAllAccountsDetails.access_level == 3)
                        {
                            bool checkAttendance = await context.Set<ProductivityManagement>()
                                .AnyAsync();
                            if (checkAttendance)
                            {
                                bool checkStudentProductivity = await context.Set<ProductivityManagement>()
                                    .AnyAsync(x =>
                                        x.accountId == findAllAccountsDetails.id && x._status == Status.TIME_OUT);
                                if (checkStudentProductivity)
                                {
                                    using (HttpClient httpClient = new HttpClient())
                                    {
                                        
                                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string responseContent = await response.Content.ReadAsStringAsync();
                                            WorldTimeApiResponse worldTimeApiResponse =
                                                JsonSerializer.Deserialize<WorldTimeApiResponse>(responseContent);
                                            if (DateTimeOffset.TryParse(worldTimeApiResponse.dateTime,
                                                    out DateTimeOffset dateTimeOffset))
                                            {
                                                TimeSpan currentTime = dateTimeOffset.TimeOfDay;
                                                ProductivityManagement productivityManagement = new ProductivityManagement();
                                                productivityManagement.accountId = findAllAccountsDetails.id;
                                                productivityManagement._productivityStatus = ProductivityStatus.PENDING;
                                                productivityManagement.TimeIn = currentTime;
                                                productivityManagement.TimeOut = TimeSpan.Zero;
                                                productivityManagement._status = Status.TIME_IN;
                                                productivityManagement.Date = dateTimeOffset.DateTime;
                                                await context.Set<ProductivityManagement>().AddAsync(productivityManagement);
                                                await context.SaveChangesAsync();
                                            }
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                // insert all new attendance
                                 using (HttpClient httpClient = new HttpClient())
                                    {
                                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                                        if (response.IsSuccessStatusCode)
                                        {
                                            string responseContent = await response.Content.ReadAsStringAsync();
                                            WorldTimeApiResponse worldTimeApiResponse =
                                                JsonSerializer.Deserialize<WorldTimeApiResponse>(responseContent);
                                            if (DateTimeOffset.TryParse(worldTimeApiResponse.dateTime,
                                                    out DateTimeOffset dateTimeOffset))
                                            {
                                                TimeSpan currentTime = dateTimeOffset.TimeOfDay;
                                                
                                                ProductivityManagement productivityManagement = new ProductivityManagement();
                                                productivityManagement.accountId = findAllAccountsDetails.id;
                                                productivityManagement._productivityStatus = ProductivityStatus.PENDING;
                                                productivityManagement.TimeIn = currentTime;
                                                productivityManagement.TimeOut = TimeSpan.Zero;
                                                productivityManagement._status = Status.TIME_IN;
                                                productivityManagement.Date = dateTimeOffset.DateTime;
                                                await context.Set<ProductivityManagement>().AddAsync(productivityManagement);
                                                await context.SaveChangesAsync();
                                            }
                                        }
                                    }
                            }
                        }

                        await PostActionsLogger(new ActionsLogger()
                        {
                            accountId = findAllAccountsDetails.id,
                            actionsMessage = "This account has been logged in",
                            created_at = DateTime.Now,
                            updated_at = DateTime.Now
                        });
                        var dictateReferences = await context.Set<TEntity>()
                            .Where(x => x.username == loginParams.username && x.status == 1).Select(x => new
                            {
                                firstname = x.firstname,
                                middlename = x.middlename,
                                lastname = x.lastname,
                                id = x.id,
                                section = x.section,
                                username = x.username,
                                mobile_number = x.mobileNumber,
                                imgurl = x.imgurl,
                                access_level = x.access_level,
                                verified = x.verified,
                                email = x.email
                            }).ToListAsync();
                        expandedObj.TokenInfo = new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            RefreshToken = refreshToken,
                            Expiration = token.ValidTo
                        };
                        expandedObj.status = 200;
                        expandedObj.references = dictateReferences;
                        return expandedObj;
                    }
                    return "UNAUTHORIZED";
                }
            }
            else
            {
                return "INVALID_PASSWORD";
            }
        } 
        else
        {
            return "ACCOUNT_DISABLED";
        }
        return 200;
    }

    public async Task<dynamic> Revoke(string username)
    {
       var user = await _userManager.FindByNameAsync(username);
        if (user == null) return "must_bad_req";
        var foundAccountFromDB = await context.Set<TEntity>()
            .Where(x => x.username == username).FirstOrDefaultAsync();
        await PostActionsLogger(new ActionsLogger()
        {
            accountId = foundAccountFromDB.id,
            actionsMessage = "This account has been logged out",
            created_at = DateTime.Now,
            updated_at = DateTime.Now
        });
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        return 200;
    }

    public async Task<List<TEntity>> ListOfAccounts(List<int> access_levels)
    {
        List<TEntity> accounts = await context.Set<TEntity>()
            .Where(entity => access_levels.Contains(entity.access_level)).ToListAsync();
        return accounts;
    }

    public async Task<dynamic> AccountDeletionToArchive(int id)
    {
        bool AccountExists = await context.Set<TEntity>()
            .AnyAsync(x => x.id == id);
        if(AccountExists)
        {
            var AccountToArchive = await context.Set<TEntity>()
                .Where(x => x.id == id).FirstOrDefaultAsync();
            AccountToArchive.status = 0;
            await context.SaveChangesAsync();
            return 200;
        }
        return 403;
    }

    public async Task<dynamic> AccountRecoverFromArchive(int id)
    {
        bool AccountExists = await context.Set<TEntity>()
            .AnyAsync(x => x.id == id);
        if (AccountExists)
        {
            var AccountToArchive = await context.Set<TEntity>()
                .Where(x => x.id == id).FirstOrDefaultAsync();
            AccountToArchive.status = 1;
            await context.SaveChangesAsync();
            return 200;
        }
        return 403;
    }

    public async Task<dynamic> AccountResendOTPRequest(AccountResendOtpParams accountResendOtpParams)
    {
        bool findAnyExistingVerifications = await context.Set<Verification>()
            .AnyAsync (x => x.email == accountResendOtpParams.email && x.isValid == 1);
        if (findAnyExistingVerifications)
        {
            int code = int.Parse(GenerateVerificationCode.GenerateCode());
            var foundExistingVerification = await context.Set<Verification>()
                .Where(x => x.email == accountResendOtpParams.email && x.isValid == 1).FirstOrDefaultAsync();
            /* verification resend count validation can do here. */
            foundExistingVerification.code = code;
            foundExistingVerification.resendCount = foundExistingVerification.resendCount + 1;
            await context.SaveChangesAsync();
            await SendEmailSMTPWithCode(new SendEmailHelper()
            {
                email = accountResendOtpParams.email,
                code = code,
                body = "This is your activation code"
            });
            return 200;
        } 
        else
        {
            int code = int.Parse(GenerateVerificationCode.GenerateCode());
            Verification verification = new Verification();
            verification.email = accountResendOtpParams.email;
            verification.isValid = 1;
            verification.resendCount = 1;
            verification.code = code;
            verification.type = "email";
            verification.createdAt = DateTime.Now;
            verification.updatedAt = DateTime.Now;
            await context.Set<Verification>().AddAsync(verification);
            await context.SaveChangesAsync();
            await SendEmailSMTPWithCode(new SendEmailHelper()
            {
                email = accountResendOtpParams.email,
                code = code,
                body = "This is your activation code"
            });
            return 200;
        }
    }

    public async Task<dynamic> CreateStudent(TEntity account)
    {
        bool foundAccountStudent = await context.Set<TEntity>()
            .AnyAsync(x => x.username == account.username || x.email == account.email && x.access_level == 3);
        var accountExists = await _userManager.FindByEmailAsync(account.email);
        if (foundAccountStudent)
        {
            return 403;
        }
        else
        {
            if (accountExists != null)
                return 403;
            JWTIdentity user = new()
            {
                Email = account.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = account.username,
            };
            Verification verification = new Verification();
            bool isThereAnyVerificationsFromDB = await context.Set<Verification>()
                .AnyAsync(x => x.email == account.email && x.isValid == 1);

            if (isThereAnyVerificationsFromDB)
            {
                int code = int.Parse(GenerateVerificationCode.GenerateCode());
                var whenVerificationsExists = await context.Set<Verification>()
                .Where(x => x.email == account.email && x.isValid == 1).FirstOrDefaultAsync();

                whenVerificationsExists.resendCount = whenVerificationsExists.resendCount + 1;
                whenVerificationsExists.code = code;
                var result = await _userManager.CreateAsync(user, account.password);
                if (!result.Succeeded)
                    return "password_too_weak";

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.password);
                account.password = hashedPassword;
                account.access_level = 3;
                account.section = account.section;
                account.course_id = account.course_id;
                account.status = 1;
                account.verified = 0;
                account.isNewAccount = 1;
                account.mobileNumber = account.mobileNumber;
                account.imgurl = "no-image-attached";
                account.created_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                account.updated_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                await context.Set<TEntity>().AddAsync(account);
                await context.SaveChangesAsync();
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = account.email,
                    code = code,
                    body = "This is your activation code"
                });
                return 200;
            }
            else
            {
                int code = int.Parse(GenerateVerificationCode.GenerateCode());
                var result = await _userManager.CreateAsync(user, account.password);
                if (!result.Succeeded)
                    return "password_too_weak";
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(account.password);
                account.password = hashedPassword;
                account.access_level = 3;
                account.section = account.section;
                account.course_id = account.course_id;
                account.status = 1;
                account.verified = 0;
                account.isNewAccount = 1;
                account.mobileNumber = account.mobileNumber;
                account.imgurl = "no-image-attached";
                account.created_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                account.updated_at = Convert.ToDateTime(System.DateTime.Now.ToString("MM/dd/yyyy"));
                verification.email = account.email;
                verification.code = code;
                verification.resendCount = 1;
                verification.isValid = 1;
                verification.type = "email";
                verification.createdAt = DateTime.Now;
                verification.updatedAt = DateTime.Now;
                await context.Set<Verification>().AddAsync(verification);
                await context.Set<TEntity>().AddAsync(account);
                await context.SaveChangesAsync();
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = account.email,
                    code = code,
                    body = "This is your activation code"
                });
                return 200;
            }
        }
    }

    public async Task<List<TEntity>> GetAccountById(int id)
    {
        List<TEntity> account = await context.Set<TEntity>()
            .Where(x => x.id == id).ToListAsync();
        return account;
    }

    public async Task<List<ProductivityManagement>> findProductivity(int id)
    {
        var list = await context.Set<ProductivityManagement>()
            .Where(x => x.accountId == id && x._status == Status.TIME_IN)
            .ToListAsync();
        return list;
    }

    public async Task<dynamic> TimeoutProductivity(int accountId)
    {
        string apiUrl = "https://timeapi.io/api/Time/current/zone?timeZone=Asia/Manila";
        bool lookForTimeInToday = await context.Set<ProductivityManagement>()
            .AnyAsync(x => x.accountId == accountId && x._status == Status.TIME_IN);
        if (lookForTimeInToday)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    WorldTimeApiResponse worldTimeApiResponse =
                        JsonSerializer.Deserialize<WorldTimeApiResponse>(responseContent);
                    if (DateTimeOffset.TryParse(worldTimeApiResponse.dateTime,
                            out DateTimeOffset dateTimeOffset))
                    {
                        TimeSpan currentTime = dateTimeOffset.TimeOfDay;
                        
                        var needToTimeOut = await context.Set<ProductivityManagement>()
                            .Where(x => x.accountId == accountId && x._status == Status.TIME_IN)
                            .FirstOrDefaultAsync();
                        needToTimeOut.TimeOut = currentTime;
                        needToTimeOut._status = Status.TIME_OUT;
                        await context.SaveChangesAsync();
                        return 200;
                    }
                }

                return 400;
            }
        }
        else
        {
            return 403;
        }
    }

    public async Task<dynamic> CheckCurrentPassword(int uuid, string password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            var getPasswordFromAccountExists = await context.Set<TEntity>()
                .Where(x => x.id == uuid)
                .FirstOrDefaultAsync();
            if (getPasswordFromAccountExists != null)
            {
                string getHashedPassword = getPasswordFromAccountExists.password;
                if (BCrypt.Net.BCrypt.Verify(password, getHashedPassword))
                {
                    return "SUCCESS";
                }
                else
                {
                    return "INVALID_PASSWORD";
                }
            }
            else
            {
                return 500;
            }
        }
        else
        {
            return "EMPTY_PASSWORD";
        }
    }

    public async Task<dynamic> ChangeBasicOrPrimaryDetails(ProfileBasicDetails profileBasicDetails)
    {
        dynamic expandedObj = new ExpandoObject();
        if (profileBasicDetails.isAuthorized)
        {
            bool checkExistsAccount = await context.Set<TEntity>()
                .AnyAsync(x => x.id == profileBasicDetails.id);
            if (checkExistsAccount)
            {
                
                var foundExistingAccount = await context.Set<TEntity>()
                    .Where(x => x.id == profileBasicDetails.id
                                && x.status == 1 && x.verified == 1)
                    .FirstOrDefaultAsync();
                 foundExistingAccount.firstname = string.IsNullOrEmpty(profileBasicDetails.firstname)
                        ? foundExistingAccount.firstname
                        : profileBasicDetails.firstname;
                    foundExistingAccount.lastname = string.IsNullOrEmpty(profileBasicDetails.lastname)
                        ? foundExistingAccount.lastname
                        : profileBasicDetails.lastname;
                    foundExistingAccount.imgurl = profileBasicDetails.imgurl;
                    if (foundExistingAccount.username != profileBasicDetails.username)
                    {
                        var updaterUsernameJwt = await _userManager.FindByNameAsync(foundExistingAccount.username);
                        bool checkIfUsernameExist = await context.Set<TEntity>()
                            .AnyAsync(x => x.username == profileBasicDetails.username);
                        if (checkIfUsernameExist)
                        {
                            return 403;
                        }

                        if (updaterUsernameJwt != null)
                        {
                            updaterUsernameJwt.UserName = profileBasicDetails.username;
                            updaterUsernameJwt.NormalizedUserName = profileBasicDetails.username;
                            await _userManager.UpdateAsync(updaterUsernameJwt);
                        }
                    }
                    foundExistingAccount.username = string.IsNullOrEmpty(profileBasicDetails.username)
                        ? foundExistingAccount.username
                        : profileBasicDetails.username;
                    if (foundExistingAccount.email != profileBasicDetails.email)
                    {
                        var updaterJwt = await _userManager.FindByEmailAsync(foundExistingAccount.email);
                       
                        bool checkIfEmailIsExist = await context.Set<TEntity>()
                            .AnyAsync(x => x.email == profileBasicDetails.email);
                        
                        if (checkIfEmailIsExist)
                        {
                            return 403;
                        }

                        if (updaterJwt != null)
                        {
                            updaterJwt.Email = profileBasicDetails.email;
                            updaterJwt.NormalizedEmail = profileBasicDetails.email;
                            var updateResult = await _userManager.UpdateAsync(updaterJwt);
                            if (updateResult.Succeeded)
                            {
                                int code = int.Parse(GenerateVerificationCode.GenerateCode());
                                Verification verification = new Verification();
                                var checkVerification = await context.Set<Verification>()
                                    .Where(x => x.email == profileBasicDetails.email
                                                   && x.isValid == 1).FirstOrDefaultAsync();
                                if (checkVerification != null)
                                {
                                    checkVerification.code = code;
                                    checkVerification.resendCount = checkVerification.resendCount + 1;
                                    foundExistingAccount.email = string.IsNullOrEmpty(profileBasicDetails.email)
                                        ? foundExistingAccount.email
                                        : profileBasicDetails.email;
                                    foundExistingAccount.verified = 0;
                                    await SendEmailSMTPWithCode(new SendEmailHelper()
                                    {
                                        email = profileBasicDetails.email,
                                        code = code,
                                        body = "STI Monitoring System Account Activation Code"
                                    });
                                    expandedObj.logoutRequired = true;
                                }
                                else
                                {
                                    verification.email = profileBasicDetails.email;
                                    verification.code = code;
                                    verification.resendCount = 1;
                                    verification.isValid = 1;
                                    verification.type = "email";
                                    verification.createdAt = DateTime.Now;
                                    verification.updatedAt = DateTime.Today;
                                    await context.Set<Verification>().AddAsync(verification);
                                    foundExistingAccount.email = string.IsNullOrEmpty(profileBasicDetails.email)
                                        ? foundExistingAccount.email
                                        : profileBasicDetails.email;
                                    foundExistingAccount.verified = 0;
                                    
                                    await SendEmailSMTPWithCode(new SendEmailHelper()
                                    {
                                        email = profileBasicDetails.email,
                                        code = code,
                                        body = "STI Monitoring System Account Activation Code"
                                    });
                                    expandedObj.logoutRequired = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        foundExistingAccount.email = string.IsNullOrEmpty(profileBasicDetails.email)
                            ? foundExistingAccount.email
                            : profileBasicDetails.email;
                    }
                    await context.SaveChangesAsync();
                    var dictateReferences = await context.Set<TEntity>()
                        .Where(x => x.id == profileBasicDetails.id && x.status == 1 && x.verified == 1 || x.verified == 0).Select(x => new
                        {
                            firstname = x.firstname,
                            middlename = x.middlename,
                            lastname = x.lastname,
                            id = x.id,
                            section = x.section,
                            username = x.username,
                            mobile_number = x.mobileNumber,
                            imgurl = x.imgurl,
                            access_level = x.access_level,
                            verified = x.verified,
                            email = x.email
                        }).ToListAsync();
                    expandedObj.status = 200;
                    expandedObj.references = dictateReferences;
                    return expandedObj;
            }
            else
            {
                return 400;
            }
        }
        else
        {
            bool checkExistsAccount = await context.Set<TEntity>()
                .AnyAsync(x => x.id == profileBasicDetails.id);
            if (checkExistsAccount)
            {
                var foundExistingAccount = await context.Set<TEntity>()
                    .Where(x => x.id == profileBasicDetails.id
                                && x.status == 1 && x.verified == 1)
                    .FirstOrDefaultAsync();
                foundExistingAccount.firstname = string.IsNullOrEmpty(profileBasicDetails.firstname)
                    ? foundExistingAccount.firstname
                    : profileBasicDetails.firstname;
                foundExistingAccount.lastname = string.IsNullOrEmpty(profileBasicDetails.lastname)
                    ? foundExistingAccount.lastname
                    : profileBasicDetails.lastname;
                foundExistingAccount.imgurl = profileBasicDetails.imgurl;
                await context.SaveChangesAsync();
                var dictateReferences = await context.Set<TEntity>()
                    .Where(x => x.id == profileBasicDetails.id && x.status == 1 && x.verified == 1).Select(x => new
                    {
                        firstname = x.firstname,
                        middlename = x.middlename,
                        lastname = x.lastname,
                        id = x.id,
                        section = x.section,
                        username = x.username,
                        mobile_number = x.mobileNumber,
                        imgurl = x.imgurl,
                        access_level = x.access_level,
                        verified = x.verified,
                        email = x.email
                    }).ToListAsync();
                expandedObj.status = 200;
                expandedObj.references = dictateReferences;
                return expandedObj;
            }

            return 401;
        }
    }

    public async Task<dynamic> RemoveAttachedImageFromDB(int uuid)
    {
        dynamic expandedObj = new ExpandoObject();
        var foundAccount = await context.Set<TEntity>()
            .AnyAsync(x => x.id == uuid);
        if (foundAccount)
        {
            var foundAttachedImage = await context.Set<TEntity>()
                .Where(x => x.id == uuid && x.status == 1 && x.verified == 1)
                .FirstOrDefaultAsync();
            foundAttachedImage.imgurl = "no-image";
            await context.SaveChangesAsync();
            var dictateReferences = await context.Set<TEntity>()
                .Where(x => x.id == uuid && x.status == 1 && x.verified == 1).Select(x => new
                {
                    firstname = x.firstname,
                    middlename = x.middlename,
                    lastname = x.lastname,
                    id = x.id,
                    section = x.section,
                    username = x.username,
                    mobile_number = x.mobileNumber,
                    imgurl = x.imgurl,
                    access_level = x.access_level,
                    verified = x.verified,
                    email = x.email
                }).ToListAsync();
            expandedObj.status = 200;
            expandedObj.references = dictateReferences;
            return expandedObj;
            
        }
        else
        {
            return 400;
        }
    }

    public async Task<dynamic> SecurityChangePassword(SecurityAndPassword securityAndPassword)
    {
        bool accountCheckFirst = await context.Set<TEntity>()
            .AnyAsync(x => x.id == securityAndPassword.id);
        if (accountCheckFirst)
        {
            var accountStorage = await context.Set<TEntity>()
                .Where(x => x.id == securityAndPassword.id)
                .FirstOrDefaultAsync();
            if (BCrypt.Net.BCrypt.Verify(securityAndPassword.currentPassword, accountStorage.password))
            {
                var user = await _userManager.FindByEmailAsync(securityAndPassword.email);
                if (user == null) throw new ArgumentNullException(nameof(user));
                accountStorage.password = BCrypt.Net.BCrypt.HashPassword(securityAndPassword.newPassword);
                accountStorage.isNewAccount = 0;
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, securityAndPassword.newPassword);
                if (!result.Succeeded)
                    return 401;
                await context.SaveChangesAsync();
                return 200;
            }
            else
            {
                return 401;
            }
        }
        else
        {
            return 400;
        }
    }

    public async Task<dynamic> StudentAttendanceReport(int section)
    {
        var studentAttendance = await context.ProductivityManagements
            .OrderByDescending(item => item.Date)
            .Join(context.AccountsEnumerable,
                joined => joined.accountId,
                account => account.id,
                (productivity, accounts) => new
                {
                    Productivity = productivity,
                    Account = accounts
                })
            .Where(x => x.Account.access_level == 3 && x.Account.status == 1
                                                    && x.Account.verified == 1 && x.Account.section == section)
            .ToListAsync();
        return studentAttendance;
    }

    public async Task<dynamic> CurrentStudentAttendanceReport(int accountId)
    {
        var studentAttendance = await context.ProductivityManagements
            .OrderByDescending(item => item.Date)
            .Join(context.AccountsEnumerable,
                joined => joined.accountId,
                account => account.id,
                (productivity, accounts) => new
                {
                    Productivity = productivity,
                    Account = accounts
                })
            .Where(x => x.Account.id == accountId)
            .ToListAsync();
        return studentAttendance;
    }

    public async Task<dynamic> StudentAttendanceReportFilterFromAndTo(DateTime from, DateTime to, int section)
    {
        var result = await context.ProductivityManagements
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderByDescending(item => item.Date)
            .Join(context.AccountsEnumerable,
                joined => joined.accountId,
                account => account.id,
                (productivity, accounts) => new
                {
                    Productivity = productivity,
                    Account = accounts
                })
            .Where(x => x.Account.access_level == 3 && x.Account.status == 1
                                                    && x.Account.verified == 1 && x.Account.section == section)
            .ToListAsync();
        return result;
    }

    public async Task<dynamic> StudentMarkStudentStatuses(Guid id, ProductivityStatus productivityStatus)
    {
        var checkProductivityExists = await context.ProductivityManagements
            .AnyAsync(x => x.id == id);
        if (checkProductivityExists)
        {
            var foundProductivity = await context.ProductivityManagements
                .Where(x => x.id == id)
                .FirstOrDefaultAsync();
            foundProductivity._productivityStatus = productivityStatus;
            await context.SaveChangesAsync();
            return 200;
        }
        else
        {
            return 400;
        }
    }
    public async Task<dynamic> CurrentStudentAttendanceReportFilterFromAndTo(DateTime from, DateTime to, int accountId)
    {
        var result = await context.ProductivityManagements
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderByDescending(item => item.Date)
            .Join(context.AccountsEnumerable,
                joined => joined.accountId,
                account => account.id,
                (productivity, accounts) => new
                {
                    Productivity = productivity,
                    Account = accounts
                })
            .Where(x => x.Account.id == accountId)
            .ToListAsync();
        return result;
    }

    public async Task<dynamic> ForgotPasswordCheckEmailWithOTP(string email)
    {
        bool checkEmailIfExist = await context.Set<TEntity>()
            .AnyAsync(x => x.email == email);
        int code = int.Parse(GenerateVerificationCode.GenerateCode());
        Verification verification = new Verification();
        if (checkEmailIfExist)
        {
            var checkVerification = await context.Set<Verification>()
                .Where(x => x.email == email
                            && x.isValid == 1).FirstOrDefaultAsync();
            if (checkVerification != null)
            {
                if (checkVerification.resendCount >= 5)
                {
                    return 403;
                }
                else
                {
                    checkVerification.resendCount = checkVerification.resendCount + 1;
                    checkVerification.code = code;
                    await context.SaveChangesAsync();
                    await SendEmailSMTPWithCode(new SendEmailHelper()
                    {
                        email = email,
                        code = code,
                        body = "Forgot password OTP Code"
                    });
                    return 200;
                }
            }
            else
            {
                verification.email = email;
                verification.code = code;
                verification.isValid = 1;
                verification.resendCount = 0;
                verification.type = "email";
                verification.createdAt = DateTime.Today;
                verification.updatedAt = DateTime.Today;
                await context.Set<Verification>().AddAsync(verification);
                await context.SaveChangesAsync();
                await SendEmailSMTPWithCode(new SendEmailHelper()
                {
                    email = email,
                    code = code,
                    body = "Forgot password OTP Code"
                });
                return 200;
            }
        }
        else
        {
            return 404;
        }
    }

    public async Task<dynamic> ForgotPasswordOTP(OtpFp otpFp)
    {
        bool checkAccount = await context.Set<TEntity>()
            .AnyAsync(x => x.email == otpFp.email);
        if (checkAccount)
        {
            bool checkCode = await context.Set<Verification>()
                .AnyAsync(x => x.email == otpFp.email
                               && x.isValid == 1 && x.code == otpFp.code);
            if (checkCode)
            {
                var updateCodeValidation = await
                    context.Set<Verification>()
                        .Where(x => x.email == otpFp.email
                                    && x.isValid == 1)
                        .FirstOrDefaultAsync();
                updateCodeValidation.isValid = 0;
                await context.SaveChangesAsync();
                return 200;
            }
            else
            {
                return 401;
            }
        }
        else
        {
            return 404;
        }
    }

    public async Task<dynamic> ChangePasswordForgotPassword(SecurityAndPasswordFP securityAndPasswordFp)
    {
        var checkAccounts = await context.Set<TEntity>()
            .AnyAsync(x => x.email == securityAndPasswordFp.email);
        if (checkAccounts)
        {
            var foundAccount = await context.Set<TEntity>()
                .Where(x => x.email == securityAndPasswordFp.email)
                .FirstOrDefaultAsync();
            var user = await _userManager.FindByEmailAsync(securityAndPasswordFp.email);
            if (user == null) throw new ArgumentNullException(nameof(user));
            foundAccount.password = BCrypt.Net.BCrypt.HashPassword(securityAndPasswordFp.password);
            foundAccount.isNewAccount = 0;
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, securityAndPasswordFp.password);
            if (!result.Succeeded)
                return 401;
            await context.SaveChangesAsync();
            return 200;
        }
        else
        {
            return 404;
        }
    }

    public async Task<dynamic> AdminSideStudentList(int section)
    {
        if (section == 0)
        {
            var noSectionResult = await context.Set<TEntity>()
                .Where(x => x.access_level == 3)
                .ToListAsync();
            return noSectionResult;
            
        }
        else
        {
            var result = await context.Set<TEntity>()
                .Where(x => x.access_level == 3 && x.section == section)
                .ToListAsync();
            return result;
        }
    }

    public async Task<dynamic> DisableAccount(int accountId)
    {
        bool checkExistingAccount = await context.Set<TEntity>()
            .AnyAsync(x => x.id == accountId);
        if (checkExistingAccount)
        {
            var result = await context.Set<TEntity>()
                .Where(x => x.id == accountId && x.status == 1)
                .FirstOrDefaultAsync();
            result.status = 0;
            await context.SaveChangesAsync();
            return 200;
        }

        return 404;
    }

    public async Task<dynamic> WatchAccountDisabling(int accountId)
    {
        bool result = await context.Set<TEntity>()
            .AnyAsync(x => x.id == accountId && x.status == 0);
        return result;
    }

    public async Task<dynamic> PostActionsLogger(ActionsLogger actionsLogger)
    {
        await context.Set<ActionsLogger>().AddAsync(actionsLogger);
        await context.SaveChangesAsync();
        return 200;
    }

    public async Task<dynamic> GetActionsLogger(int accountId)
    {
        var result = await context.Set<ActionsLogger>()
            .Where(x => x.accountId == accountId)
            .OrderByDescending(t => t.created_at).ToListAsync();
        return result;
    }

    public async Task<dynamic> GetAccountsDetails(int accountId)
    {
        bool accountChecks = await context.Set<TEntity>()
            .AnyAsync(x => x.id == accountId);
        if (accountChecks)
        {
            var result = await context.Set<TEntity>()
                .Where(x => x.id == accountId)
                .Select(account => new
                {
                    account.id,
                    account.imgurl,
                    account.firstname,
                    account.lastname,
                    account.access_level,
                    account.course_id,
                    account.section,
                    account.email,
                    account.username,
                    account.status,
                    account.verified
                }).ToListAsync();
            return result;
        }

        return 404;
    }

    public async Task<bool> IsNewAccount(int id)
    {
        bool result = await context.Set<TEntity>().AnyAsync(x => x.id == id && x.isNewAccount == 1);
        return result;
    }

    public async Task<bool> IsNotVerified(int id)
    {
        bool AccountNotVerified = await context.Set<TEntity>()
            .AnyAsync(x => x.id == id && x.verified == 0);
        return AccountNotVerified;
    }
}