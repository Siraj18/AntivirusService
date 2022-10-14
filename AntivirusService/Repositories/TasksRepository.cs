using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using AntivirusService.Exceptions;
using AntivirusService.Models;

namespace AntivirusService.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private ConcurrentDictionary<string,ScanTask> _tasks;

        public TasksRepository()
        {
            _tasks = new();
        }

        public void AddTask(ScanTask scanTask)
        {
            _tasks[scanTask.Id] = scanTask;
        }

        public ScanTask GetScanTask(string id)
        {
            if (!_tasks.ContainsKey(id))
            {
                throw new ObjectNotFoundException();
            }

            return _tasks[id];
        }

        public void UpdateTask(ScanTask scanTask)
        {
            if (!_tasks.ContainsKey(scanTask.Id))
            {
                throw new ObjectNotFoundException();
            }
            
            _tasks[scanTask.Id] = scanTask;
        }
    }
}