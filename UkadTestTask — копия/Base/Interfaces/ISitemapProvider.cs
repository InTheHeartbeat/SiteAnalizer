using System.Collections.Generic;

namespace SiteAnalyzer.Base.Interfaces
{
    interface ISitemapProvider
    {
        List<Sitemap> GetSitemapsFromUrl(string url);
    }
}
