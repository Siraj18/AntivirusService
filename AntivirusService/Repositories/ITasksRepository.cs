using System.Threading.Tasks;
using AntivirusService.Models;

namespace AntivirusService.Repositories
{
    public interface ITasksRepository
    {
        void AddTask(ScanTask scanTask);
        ScanTask GetScanTask(string id);
        void UpdateTask(ScanTask scanTask);
    }
}