using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using System.Xml;
using System.Xml.Linq;
using SiteAnalyzer.Scanning;
using WebGrease.Css.Extensions;

namespace SiteAnalyzer.Base
{
    [Serializable]
    public class Sitemap
    {
        public string Url { get; set; }
        public List<SitemapUrl> Urls { get; set; }

        public Sitemap(string url, List<SitemapUrl> urls)
        {
            Url = url;
            Urls = urls;            
        }

        public Sitemap(string url)
        {
            Url = url;
        }

        public Sitemap()
        { }
    }
}