using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace UkadTestTask.Base
{
    [Serializable]
    public class WebSite
    {
        public string Url { get; set; }
        public List<Sitemap> Sitemaps { get; set; }

        [XmlIgnore]
        public bool SitemapLoaded => Sitemaps != null && Sitemaps.Count > 0;

        public WebSite()
        { }

        public WebSite(string url)
        {
            Url = url;
        }
    }
}