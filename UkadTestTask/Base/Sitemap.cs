using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using System.Xml;
using System.Xml.Linq;
using UkadTestTask.Scanning;
using WebGrease.Css.Extensions;

namespace UkadTestTask.Base
{
    [Serializable]
    public class Sitemap
    {
        public string Url { get; private set; }
        public List<ScannableUrl> Urls { get; set; }

        public Sitemap(string url, List<ScannableUrl> urls)
        {
            Url = url;
            Urls = urls;            
        }

        public Sitemap(string url)
        {
            Url = url;
        }
    }
}