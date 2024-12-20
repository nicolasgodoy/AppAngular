using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AppAngular.Domain.Models
{
    public class SmtpConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public SmtpClient GenerateSmtpClient()
        {
            SmtpClient client = new(Host, Port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(Name, Password);
            ServicePointManager.ServerCertificateValidationCallback = (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
            {
                return true;
            };
            client.EnableSsl = true;
            return client;
        }

        public SmtpConfiguration Bind(IConfiguration configuration)
        {
            IConfigurationSection smtpConfig = configuration.GetSection(nameof(SmtpConfiguration));

            if (smtpConfig.Exists())
               smtpConfig.Bind(this);

            return this;
        }
    }
}
