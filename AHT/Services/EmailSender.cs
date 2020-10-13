using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Configuration;
using AHT.Services.Interfaces;
using System.IO;
using System;
using Microsoft.AspNetCore.Html;
using AHT.Models.VM;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace AHT.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkId=532713
    public class EmailSender : IEmailSender
    {
        private readonly IViewRenderService _viewRenderService;
        private readonly EmailSettings _settings;
        public EmailSender(
            IViewRenderService viewRenderService,
            IOptions<EmailSettings> settings)
        {
            _settings = settings?.Value;
            _viewRenderService = viewRenderService;
        }
        public IConfiguration Configuration { get; }
        public Task<Response> SendEmailAsync(string email, string subject, string message) =>
            Execute(_settings.SendGridKey, subject, message, email);
        public async Task<Response> SendEmailAsync(string email, string subject, string message, Stream attachment = null, string attachmentName = null, string mimeType = null)
        {
            var client = new SendGridClient(_settings.SendGridKey);
            var From = new EmailAddress("no-responder@epicsolutions.cl", "AHT");
            var Subject = subject;
            var PlainTextContent = message;
            var HtmlContent = message;
            var to = new EmailAddress(email, "Cliente");
            var msg = MailHelper.CreateSingleEmail(From, to, Subject, PlainTextContent, HtmlContent);
            if (attachment != null)
            {
                await msg.AddAttachmentAsync(attachmentName, attachment, mimeType).ConfigureAwait(false);
            }
            return await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
        public Task<Response> SendEmailAsync(string email, string subject, string message, string attachmentPath = null, string attachmentName = null, string mimeType = null) =>
            Execute(_settings.SendGridKey, subject, message, email, attachmentPath, attachmentName, mimeType);
        public static Task<Response> Execute(string apiKey, string subject, string message, string email, string attachment = null, string attachmentName = null, string mimeType = null, string contentDisposition = null, string contentId = null)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-responder@epicsolutions.cl", "AHT"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            if(attachment != null)
            {
                msg.AddAttachment(attachmentName, attachment, mimeType, contentDisposition, contentId);
            }
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
        public async Task<Response> SendVerificationEmail(string email, Uri callbackUrl)
        {
            var logo = "wwwroot/android-chrome-192x192.png";

            var emailModel = new ValidationEmailVM
            {
                Header = new HtmlString("Sistema de Informes de Largo Telomérico<br>Confirma tu correo."),
                Url = new HtmlString(callbackUrl?.ToString()),
                Color = "324567",
                LogoId = "logo",
                Body = new HtmlString("Al hacer click en el siguiente enlace, estás confirmando tu correo."),
                ButtonTxt = new HtmlString("Confirmar Correo")
            };

            var emailView = await _viewRenderService
                .RenderToStringAsync("_ValidationEmail", emailModel)
                .ConfigureAwait(false);

            return await Execute(_settings.SendGridKey, "Verifica tu correo", emailView, email, GetByteString(logo), "logo.png", "image/png", "inline", "logo").ConfigureAwait(false);
        }
        public static string GetByteString(string path) => Convert.ToBase64String(File.ReadAllBytes(path));
    }
    public class EmailSettings
    {
        public string SendGridKey { get; set; }
    }
}
