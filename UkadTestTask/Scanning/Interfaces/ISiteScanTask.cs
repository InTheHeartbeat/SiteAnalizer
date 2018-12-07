using System.Threading.Tasks;

namespace SiteAnalyzer.Scanning.Interfaces
{
    internal interface ISiteScanTask
    {
        Task Scan();
        void Stop();
    }
}
