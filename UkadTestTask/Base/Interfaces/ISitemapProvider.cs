using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteAnalyzer.Base.Interfaces
{
    internal interface ISitemapProvider
    {
        Task<List<Sitemap>> GetSitemapsFromUrl(string url);
    }
}
