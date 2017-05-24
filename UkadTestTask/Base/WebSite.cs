using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UkadTestTask.Base
{
    public class WebSite
    {
        public string Url { get; private set; }
        public List<Sitemap> Sitemaps { get; set; }

        public WebSite(string url)
        {
            Url = url;
        }
    }
}