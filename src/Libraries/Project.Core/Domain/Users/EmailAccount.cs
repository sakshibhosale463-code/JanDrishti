using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Domain.Users;
public class EmailAccount : BaseEntity
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }
}