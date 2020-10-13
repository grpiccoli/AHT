using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AHT.Data;
using AHT.Models;
using AHT.Models.Entities;

namespace AHT.Services
{
    public class ImportService : IImport
    {
        private string Declaration { get; set; }
        private List<string> Headers { get; set; }
        private Dictionary<string, Dictionary<string, int>> InSet { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        private MethodInfo FirstOrDefaultAsyncMethod { get; set; }
        [SuppressMessage("Performance", "CA1802:Use literals where appropriate", Justification = "includes circular definition")]
        private static readonly BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.Public;
        private List<PropertyInfo> FieldInfos { get; } = new List<PropertyInfo>();
        //private const string encoding = "Windows-1252";
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<ImportService> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;
        public ImportService(ApplicationDbContext context,
            IHttpContextAccessor httpContext,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<ImportService> localizer)
        {
            _httpContext = httpContext;
            _userManager = userManager;
            _localizer = localizer;
            _context = context;
        }
        public async Task<Task> AddAsync(IFormFile file) =>
            await AddAsync(file?.OpenReadStream()).ConfigureAwait(false); //polymorphism convertion
        public async Task<Task> AddAsync(Stream file)
        {
            //var matrix = await _tableToExcel.HtmlTable2Matrix(file).ConfigureAwait(false);
            //return await AddEntryAsync(matrix).ConfigureAwait(false);
            throw new ArgumentException(_localizer[$"Unknown Error"]);
        }
    }
}