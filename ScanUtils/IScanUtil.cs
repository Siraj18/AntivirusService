using System.Threading.Tasks;

namespace ScanUtils
{
    public interface IScanUtil
    {
        Task<ScanResult> ScanDirectory(string directory);
    }
}