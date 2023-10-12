using System.Dynamic;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using RestSharp;
using SendGrid;
using SendGrid.Helpers.Mail;
using sti_sys_backend.Core.Services;
using sti_sys_backend.DataImplementations;
using sti_sys_backend.DB;
using sti_sys_backend.Models;
using sti_sys_backend.Utilization;
using MailSettings = sti_sys_backend.Utilization.MailDto.MailSettings;

namespace sti_sys_backend.Core.ServiceImplementations;

public abstract class VerificationWithCooldownImpl<TEntity, TContext> : IVerificationService<TEntity>
    where TEntity: class, IVerification
    where TContext : DatabaseQueryable

{
    private readonly TContext context;
    private readonly MailSettings _mailSettings;

    public VerificationWithCooldownImpl(TContext context, IOptions<MailSettings> mailSettings)
    {
        this.context = context;
        _mailSettings = mailSettings.Value;
    }

    public async Task<dynamic> CheckVerificationCode(int code, string email, string? type = "account_activation")
    {
        var verifyCode = await context.Set<TEntity>().AnyAsync(a => a.code == code && a.email == email && a.isValid == 1);
        var findVerifiedByReference = await context.Set<TEntity>().Where(x => x.code == code && x.email == email && x.isValid == 1).FirstOrDefaultAsync();
        var findAccountsByEmail = await context.AccountsEnumerable.AnyAsync(x => x.email == email && x.verified == 0);
        var GetAccountsDetailsByEmail = await context.AccountsEnumerable.Where(x => x.email == email).FirstOrDefaultAsync();
        var FindAccountsByFp = await context.AccountsEnumerable.AnyAsync(x => x.email == email);

        if (verifyCode)
        {
            if(type == "account_activation")
            {
                if(findAccountsByEmail)
                {
                    findVerifiedByReference.isValid = 0;
                    GetAccountsDetailsByEmail.verified = 1;
                    await context.SaveChangesAsync();
                    return 200;
                } 
                else
                {
                    findVerifiedByReference.isValid = 0;
                    await context.SaveChangesAsync();
                    return 200;
                }
            }
            else
            {
                if (FindAccountsByFp)
                {
                    findVerifiedByReference.isValid = 0;
                    await context.SaveChangesAsync();
                    return 200;
                } 
                else
                {
                    return 403;
                }
            }
        } 
        else
        {
            return 402;
        }
    }

    public async Task<dynamic> CheckVerificationCountsWhenLoad(string email)
    {
        var selectedResentCounts = await context.Set<TEntity>()
            .Where(x => x.email == email && x.isValid == 1)
            .FirstOrDefaultAsync();
        if (selectedResentCounts != null)
        {
            return selectedResentCounts.resendCount;
        }
        else
        {
            return 400;
        }
    }

    public async Task<dynamic> PostNewVerificationCooldowns(VerificationCooldown verificationCooldown)
    {
        var checkverificationEdition = await context.Set<VerificationCooldown>()
            .AnyAsync(x => x.resendCount == verificationCooldown.resendCount);
        VerificationCooldown vc = new VerificationCooldown();
        if (checkverificationEdition)
        {
            return 501;
        }
        else
        {
            vc.resendCount = verificationCooldown.resendCount;
            vc.cooldown = verificationCooldown.cooldown;
            await context.Set<VerificationCooldown>().AddAsync(vc);
            await context.SaveChangesAsync();
            return 200;
        }
    }

    public async Task<dynamic> ResendVerificationCode(string type, string email)
    {
        bool findAnyVerification = await context.Set<TEntity>().AnyAsync();
        
        if (findAnyVerification)
        {
            var sentCount = await context.Set<TEntity>()
            .Where(x => x.email == email && x.isValid == 1).FirstOrDefaultAsync();
            var matchSentCountWithCooldown = await context.Set<VerificationCooldown>()
            .AnyAsync(x => x.resendCount == sentCount.resendCount);
            dynamic dynobj = new ExpandoObject();
            if (type == "email")
            {
                if (matchSentCountWithCooldown)
                {
                    var findCooldown = await context.Set<VerificationCooldown>()
                        .Where(x => x.resendCount == sentCount.resendCount).FirstOrDefaultAsync();
                    int code = int.Parse(GenerateVerificationCode.GenerateCode());
                    sentCount.code = code;
                    sentCount.resendCount = sentCount.resendCount + 1;
                    sentCount.createdAt = DateTime.Today;
                    await SendEmailSMTPWithCode(
                        new SendEmailHelper()
                        {
                            email = email,
                            code = code,
                            body= "Kindly use this code to verify your account"
                        }
                    );
                    await context.SaveChangesAsync();
                    dynobj.cooldown = findCooldown.cooldown;
                    dynobj.status = 401;
                    return dynobj;
                }
                else
                {
                    if (sentCount.resendCount >= 5)
                    {
                        return 400;
                    }
                    else
                    {
                        int code = int.Parse(GenerateVerificationCode.GenerateCode());
                        sentCount.code = code;
                        sentCount.resendCount = sentCount.resendCount + 1;
                        sentCount.createdAt = DateTime.Today;
                        await SendEmailSMTPWithCode(
                            new SendEmailHelper()
                            {
                                email = email,
                                code = code,
                                body= "Kindly use this code to verify your account"
                            }
                        );
                        await context.SaveChangesAsync();
                        return 200;
                    }
                }
            }
            else
            {
                return "sms-provider";
            }
        }
        else
        {
            int code = int.Parse(GenerateVerificationCode.GenerateCode());
            Verification verification = new Verification();
            verification.email = email;
            verification.code = code;
            verification.resendCount = 1;
            verification.isValid = 1;
            verification.type = type;
            verification.createdAt = Convert.ToDateTime(System.DateTime.Today);
            verification.updatedAt = Convert.ToDateTime(System.DateTime.Today);
            await context.Verifications.AddAsync(verification);
            await context.SaveChangesAsync();
            await SendEmailSMTPWithCode(
                new SendEmailHelper()
                {
                    email = email,
                    code = code,
                    body= "Kindly use this code to verify your account"
                }
            );
            return 200;
        }
    }

    public async Task SendEmailSMTPWithCode(SendEmailHelper sendEmailHelper)
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
        using var smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(mail);
        smtp.Disconnect(true);
        /*var rGetKey = await context.Set<MailGunSecuredApiKey>()
            .Where(x => x._apistatus == ApiStatus.ACTIVE)
            .FirstOrDefaultAsync();
        var apiKey = rGetKey.domain;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("bertserrano67@gmail.com", "STI System");
        var subject = "STI System Notification";
        var to = new EmailAddress(sendEmailHelper.email, "User");
        var plainTextContent = "STI SYSTEM NOTIFICATIONS";
        var htmlContent = @"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>OTP Email</title>
</head>
<body>
    <div style=""text-align: center;"">
        <h1>Your OTP Code</h1>
        <p>Use the following code to verify your account:</p>
        <h2 style=""color: #007bff;"">" + sendEmailHelper.code + @"</h2>
    </div>
</body>
</html>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);*/
    }
}