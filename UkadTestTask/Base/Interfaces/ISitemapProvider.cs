using System.Collections.Generic;

namespace UkadTestTask.Base.Interfaces
{
    interface ISitemapProvider
    {
        List<Sitemap> GetSitemapsFromUrl(string url);
    }
}
