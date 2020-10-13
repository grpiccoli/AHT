using Microsoft.AspNetCore.Http;
using SendGrid;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AHT.Services.Interfaces
{
    public interface IEmailSender
    {
        Task<Response> SendEmailAsync(string email, string subject, string message);
        Task<Response> SendEmailAsync(string email, string subject, string message, string attachment, string attachmentName, string mimeType);
        Task<Response> SendVerificationEmail(string email, Uri callbackUrl);
        Task<Response> SendEmailAsync(string email, string subject, string message, Stream attachment = null, string attachmentName = null, string mimeType = null);
    }
}
