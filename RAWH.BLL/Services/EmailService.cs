using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RAWH.BLL.Interfaces;
using RAWH.BLL.Setting;
namespace RAWH.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly emailSetting _emailSetting;

        public EmailService(IOptions<emailSetting> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }
        public async Task<string> sendEmail(string email, string message)
        {
            try
            {

                using (var smtp = new SmtpClient())

                {
                    smtp.Connect(_emailSetting.Host, _emailSetting.Port);
                    smtp.Authenticate(_emailSetting.Email, _emailSetting.Password);
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(_emailSetting.DisplayName, _emailSetting.Email));
                    mimeMessage.To.Add(MailboxAddress.Parse(email));
                    mimeMessage.Subject = "Your Code";
                    var body = new BodyBuilder
                    {
                        HtmlBody = message
                    };
                    mimeMessage.Body = body.ToMessageBody();
                    await smtp.SendAsync(mimeMessage);
                    return "Email sent successfully!";
                }
            }
            catch (Exception ex)
            {
                return $"Email sending failed: {ex.Message}";
            }
        }
    }
}
