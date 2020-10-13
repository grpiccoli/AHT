using System;
using System.IO;
using System.Threading.Tasks;
using AHT.Services;
using AHT.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AHT.Controllers
{
    public class ApiController : Controller
    {
        private readonly IImport _import;
        private readonly IEmailSender _emailSender;
        public ApiController(IImport import,
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _import = import;
        }

        [HttpPost]
        [Produces("application/json")]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> SubmitFile(IFormFile qqfile)
        {
            if (qqfile == null) return Json(new { success = false, error = "error file null" });

            if (qqfile.Length > 0)
            {
                try
                {
                    using var ms = new MemoryStream();
                    qqfile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);

                    await _emailSender.SendEmailAsync(
                        email: "contacto@epicsolutions.cl", 
                        subject: "AHT Requerimiento", 
                        message: "",
                        attachment: s,
                        "resultado.xlsx", 
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet").ConfigureAwait(false);
                    //var result = await _import.AddAsync(qqfile).ConfigureAwait(false);
                    return Json(new { success = true, error = string.Empty });
                }
                catch
                {
                    throw;
                    //return Json(new { success = false, error = ex.Message });
                }
            }
            throw new ArgumentNullException($"Argument {qqfile} has length 0");
            //return Json(new { success = false, error = "qqfile length 0" });
        }
    }
}
