using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace AppAngular.Service.Servicios
{
    public class EnviarEmailService : IEnviarEmailService
    {
        private readonly IFluentEmailFactory _mail;
        private readonly SmtpConfiguration _smtpConfiguration;

        public EnviarEmailService(IFluentEmailFactory mail,
            IConfiguration configuration)
        {
            _mail = mail;
            _smtpConfiguration = new SmtpConfiguration().Bind(configuration);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                await _mail
                        .Create()
                        .To(email)
                        .SetFrom(_smtpConfiguration.Address)
                        .Subject(subject)
                        .Body(htmlMessage, true)
                        .SendAsync();
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }

        public async Task SendEmailAsync(IEnumerable<string> emails, string subject, string htmlMessage)
        {
            await _mail
                     .Create()
                     .To(GetAddresses(emails))
                     .SetFrom(_smtpConfiguration.Address)
                     .Subject(subject)
                     .Body(htmlMessage)
            .SendAsync();
        }

        private List<Address> GetAddresses(IEnumerable<string> emails)
        {
            return emails.Select(email => new Address(email)).ToList();
        }
    }
}
