using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkadTestTask.Base
{
    interface ISitemapProvider
    {
        Sitemap GetSitemapFromUrl(string url);
    }
}
