using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Shared.MailService
{
    public interface IMailService
    {
        void Send(string subject, string body, object bodyParams, string[] recipients);
    }
}
