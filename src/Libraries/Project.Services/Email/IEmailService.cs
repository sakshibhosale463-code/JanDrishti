using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.Domain.Users;

namespace Project.Services.Email;
public interface IEmailService
{
    Task SendEmailAsync(List<string> toEmailAddress, string subject, string body);

    Task<MessageTemplate> GetEmailTemplateByNameAsync(string name);

    Task<bool> ProjectGyaanRegistrationEmail(string email, string name);

    Task SendEmailWithAttachmentAsync(List<string> toEmailAddress, string subject, string body, string attachmentPath);
}
