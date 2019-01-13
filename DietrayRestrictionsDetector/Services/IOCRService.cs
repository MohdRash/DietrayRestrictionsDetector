using System.IO;
using System.Threading.Tasks;

namespace DietrayRestrictionsDetector.Services
{
    public interface IOCRService
    {
        Task<string[]> DetectTextInImageAsync(Stream image);
    }
}
