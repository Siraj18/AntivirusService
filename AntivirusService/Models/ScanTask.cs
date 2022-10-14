using System.Text;
using ScanUtils;

namespace AntivirusService.Models
{

    public static class ScanTaskStatuses
    {
        public static string INPROGRESS = "Scan task in progress, please wait";
        public static string COMPLETED = "Task completed";
    }
    public class ScanTask
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public ScanResult ScanResult { get; set; }
    }
}