using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Project.Core.Domain.Users;
using Project.Data;

namespace Project.Services.Email;
public class EmailService : IEmailService
{
    #region Fields

    private readonly IRepository<EmailAccount> _emailAccountRepository;
    private readonly IRepository<MessageTemplate> _messageTemplateRepository;
    private readonly IConfiguration _configuration;
    #endregion

    #region Constructor

    public EmailService(IRepository<EmailAccount> emailAccountRepository,
        IRepository<MessageTemplate> messageTemplateRepository,
        IConfiguration configuration)
    {
        _emailAccountRepository = emailAccountRepository;
        _messageTemplateRepository = messageTemplateRepository;
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public async Task SendEmailAsync(List<string> toEmailAddress, string subject, string body)
    {
        //emailAccountdetails
        var emailAccount = await _emailAccountRepository.Table.FirstOrDefaultAsync();

        var message = new MailMessage
        {
            From = new MailAddress(emailAccount.Email, emailAccount.DisplayName)
        };
        foreach (var toEmail in toEmailAddress)
        {
            message.To.Add(new MailAddress(toEmail, emailAccount.DisplayName ?? toEmail));
        }

        //content
        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        //send email
        using (var smtpClient = new SmtpClient())
        {
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Host = emailAccount.Host;
            smtpClient.Port = emailAccount.Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(emailAccount.UserName, emailAccount.Password);
            smtpClient.Send(message);
        }
    }

    public async Task<MessageTemplate> GetEmailTemplateByNameAsync(string name)
    {
        //emailTemplate
        var emailTemplate = await _messageTemplateRepository.Table.FirstOrDefaultAsync(x => x.Name == name);
        return emailTemplate;
    }

    public async Task SendEmailWithAttachmentAsync(List<string> toEmailAddress, string subject, string body, string attachmentPath)
    {
        var emailAccount = await _emailAccountRepository.Table.FirstOrDefaultAsync();

        var message = new MailMessage
        {
            From = new MailAddress(emailAccount.Email, emailAccount.DisplayName)
        };

        foreach (var email in toEmailAddress)
        {
            message.To.Add( new MailAddress( email, emailAccount.DisplayName ?? email));
        }

        message.Subject = subject;
        message.Body = body;
        message.IsBodyHtml = true;

        if (!string.IsNullOrWhiteSpace(attachmentPath) && File.Exists(attachmentPath))
        {
            message.Attachments.Add( new Attachment(attachmentPath));
        }

        using var smtpClient = new SmtpClient
        {
            Host = emailAccount.Host,
            Port = emailAccount.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                emailAccount.UserName,
                emailAccount.Password)
        };

        await smtpClient.SendMailAsync(message);
    }

    public Task<bool> ProjectGyaanRegistrationEmail(string email, string name)
    {
        var password = $"{_configuration["emailsettings:password"]}";
        var emailid = $"{_configuration["emailsettings:email"]}";
        string htmlBody = $@"
        <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""UTF-8"">
                <title>Registration Confirmation – Project Gyaan</title>
            </head>
            <body style=""font-family: Arial, Helvetica, sans-serif; font-size: 14px; color: #333; line-height: 1.6;"">
                <p>Dear <strong>{name}</strong>,</p>
                <p>
                    Thank you for registering for <strong>Project Gyaan</strong>.
                </p>
                <p>
                    We have successfully received your application. Our team will review your details
                    and contact you shortly with the next steps.
                </p>
                <p>
                    If you have any questions, feel free to reach out to us.
                </p>
                <p>
                    Thank You,<br>
                    <strong>Team Project Gyaan</strong><br>
                    Email: info@projectgyaan.com
                </p>
            </body>
            </html>";
        var toemails = new List<string>();
        string subject = $"Registration Confirmation – Project Gyaan";

        var toRecipients = new List<string>();
        var mailMessage = new MailMessage
        {
            From = new MailAddress(emailid),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(email);

        using (var smtpClient = new SmtpClient("smtp.office365.com"))
        {
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailid, password);
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        return Task.FromResult(true);
    }



    #endregion
}
