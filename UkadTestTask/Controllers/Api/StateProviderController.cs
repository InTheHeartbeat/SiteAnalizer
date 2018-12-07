using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using SiteAnalyzer.Scanning;

namespace SiteAnalyzer.Controllers.Api
{
    public class StateProviderController : ApiController
    {
        [HttpGet]
        public ScanState GetState(string url)
        {
           return ScannerProvider.Scanner.ScanTasks.LastOrDefault(t => t.Site.Url == Encoding.UTF8.GetString(Convert.FromBase64String(url))).ResultState;
        }
    }
}
