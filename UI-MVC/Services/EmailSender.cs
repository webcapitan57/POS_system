using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using UI_MVC.Services;

namespace UI.MVC.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;


        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor, ILogger<EmailSender> logger)
        {
            _logger = logger;
            Options = optionsAccessor.Value;
            _logger.Log(LogLevel.Debug, optionsAccessor.Value.SendGridKey);
            _logger.Log(LogLevel.Debug, optionsAccessor.Value.SendGridUser);
        }

        public AuthMessageSenderOptions Options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            return Execute(Options.SendGridKey, subject, message, email);
        }


        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage
            {
                From = new EmailAddress("MakeVRTGreatAgain@gmail.com", Options.SendGridUser)
            };
            if (subject.Equals("CreateGroup"))
            {
                msg.SetTemplateId("d-b6fe4f3602d9466db0d5864ad866dc6b");
                var dynamicContent = message.Split(',');
                msg.SetTemplateData(new GroupEmail()
                {
                    GroupName = dynamicContent[0],
                    GroupCode = dynamicContent[1],
                    ReturnUrl = dynamicContent[2]
                });
            }
            else if(subject.Equals("ConfirmEmail"))
            {
                msg.SetTemplateId("d-3645202ebfb34fcd8bcd919f3fbf3f3a");
                var dynamicContent = message.Split(',');
                msg.SetTemplateData(new ValidationEmail()
                {
                    FullAccountName = dynamicContent[0],
                    AccountName = dynamicContent[1],
                    SetUpName = dynamicContent[2],
                    ReturnUrl = dynamicContent[3]
                });
            }else if (subject.Equals("Admin"))
            {
                msg.SetTemplateId("d-3f120bb26555441e823814646b390cb3");
                var dynamicContent = message.Split(',');
                msg.SetTemplateData(new AdminValidationEmail()
                {
                    Email = dynamicContent[0],
                    ReturnUrl = dynamicContent[1]
                }); 
            }
            else
            {
                msg.Subject = subject;
                msg.PlainTextContent = message;
                msg.HtmlContent = message;
            }
            
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
        private class GroupEmail
        {
            [JsonProperty("groupCode")]
            public string GroupCode { get; set; }

            [JsonProperty("groupName")]
            public string GroupName { get; set; }
            
            [JsonProperty("returnUrl")]
            public string ReturnUrl { get; set; }
        }
        
        private class ValidationEmail
        {
            [JsonProperty("fullAccountName")]
            public string FullAccountName { get; set; }

            [JsonProperty("accountName")]
            public string AccountName { get; set; }
            
            [JsonProperty("setUpName")]
            public string SetUpName { get; set; }
            
            [JsonProperty("returnUrl")]
            public string ReturnUrl { get; set; }
        }
        
        private class AdminValidationEmail
        {
            [JsonProperty("email")]
            public string Email { get; set; }
            
            [JsonProperty("returnUrl")]
            public string ReturnUrl { get; set; }
        }
    }
}