using System;
using System.IO;
using System.Threading.Tasks;
using AntivirusService.Exceptions;
using AntivirusService.Models;
using AntivirusService.Repositories;
using ScanUtils;

namespace AntivirusService.Services
{
    public class TaskService : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IScanUtil _scanUtil;

        public TaskService(ITasksRepository tasksRepository, IScanUtil scanUtil)
        {
            _tasksRepository = tasksRepository;
            _scanUtil = scanUtil;
        }

        public string ScanFolder(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new ValidationException("Directory not exists");
            }
            
            var scanTask = new ScanTask();
            scanTask.Id = Guid.NewGuid().ToString();
            scanTask.Status = ScanTaskStatuses.INPROGRESS;
            
            _tasksRepository.AddTask(scanTask);

            // запускаем задачу
            Task.Run((async () =>
            {
                var scanResult = await _scanUtil.ScanDirectory(directory);
                
                scanTask.ScanResult = scanResult;
                scanTask.Status = ScanTaskStatuses.COMPLETED;
            
                _tasksRepository.UpdateTask(scanTask);
            }));

            return scanTask.Id;
        }

        public string GetStatus(string id)
        {
            var scanTask = _tasksRepository.GetScanTask(id);

            if (scanTask.Status == ScanTaskStatuses.COMPLETED)
            {
                return scanTask.ScanResult.GenerateReport();
            }

            return scanTask.Status;
        }
    }
}