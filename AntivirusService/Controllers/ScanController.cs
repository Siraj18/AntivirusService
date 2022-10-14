using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AntivirusService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanUtils;

namespace AntivirusService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {

        private readonly ITasksService _tasksService;

        public ScanController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        public string ScanFolder(string directory)
        {
            return _tasksService.ScanFolder(directory);
        }
        
        [HttpGet]
        public string GetStatus(string id)
        {
            return _tasksService.GetStatus(id);
        }
    }
}