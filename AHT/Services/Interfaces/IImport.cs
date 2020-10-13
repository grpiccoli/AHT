using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AHT.Services
{
    public interface IImport
    {
        Task<Task> AddAsync(IFormFile file);
    }
}
