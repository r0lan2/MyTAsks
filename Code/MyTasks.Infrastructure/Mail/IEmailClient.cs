using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyTasks.Infrastructure.Mail
{
    public interface IEmailClient
    {
        SendEmailResult SendEmail(MailMessage message);
    }
}
