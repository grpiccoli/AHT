using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Configuration;
using AHT.Services.Interfaces;
using System.IO;
using System;
using Microsoft.AspNetCore.Html;
using AHT.Models.VM;

namespace AHT.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkId=532713
    public class EmailSender : IEmailSender
    {
        private readonly IViewRenderService _viewRenderService;
        public EmailSender(
            IViewRenderService viewRenderService,
            IConfiguration configuration)
        {
            Configuration = configuration;
            _viewRenderService = viewRenderService;
        }

        public IConfiguration Configuration { get; }

        public Task<Response> SendEmailAsync(string email, string subject, string message, string logo = null)
        {
            return Execute("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", subject, message, email, logo);
        }

        public static Task<Response> Execute(string apiKey, string subject, string message, string email, string logo = null)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-responder@aht.epicsolutions.cl", "AHT"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            var bytes = File.ReadAllBytes(logo);
            var file = Convert.ToBase64String(bytes);
            msg.AddAttachment("lofo.png", file, "image/png", "inline", "logo");
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

            return await SendEmailAsync(email, "Verifica tu correo", emailView, logo)
                .ConfigureAwait(false);
        }
    }
}
