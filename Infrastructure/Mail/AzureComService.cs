using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Core.Application.DTOs.Configurations;
using Core.Application.DTOs.Local;
using Core.Application.Interfaces.Mail;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mail {
    [RegisterAsSingleton]
    public class AzureComService : IEmailService {
        private EmailClient _client;
        private readonly EmailParam _emailconfig;
        public AzureComService(IOptionsMonitor<SystemVariables> config) {
            _emailconfig = config.CurrentValue.EmailParam;
            string connectionString = string.Concat("endpoint=", _emailconfig.smtpServer, ";accesskey=", _emailconfig.password);
            _client = new EmailClient(connectionString);
        }
        public async Task<bool> send(MailEnvelope envelope) {
            EmailContent emailContent = new EmailContent(envelope.subject);
            if (envelope.bodyIsPlainText) {
                emailContent.PlainText = envelope.body;
            } else {
                emailContent.Html = envelope.body;
            }
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            for (int i = 0; i < envelope.toAddress.Length; i++) {
                try {
                    EmailAddress to = new EmailAddress(envelope.toAddress[i].Trim(), envelope.toName[i]);
                    emailAddresses.Add(to);
                } catch {
                    EmailAddress to = new EmailAddress(envelope.toAddress[i].Trim());
                    emailAddresses.Add(to);
                }
            }
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage(_emailconfig.fromAddress, emailContent, emailRecipients);
            SendEmailResult emailResult = await _client.SendAsync(emailMessage);
            return true;
        }
    }
}
