using System.Threading.Tasks;

namespace AntivirusService.Services
{
    public interface ITasksService
    {
        string ScanFolder(string directory);
        string GetStatus(string id);
    }
}