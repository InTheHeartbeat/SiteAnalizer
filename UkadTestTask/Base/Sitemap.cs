using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebSockets;
using System.Xml;
using System.Xml.Linq;
using WebGrease.Css.Extensions;

namespace UkadTestTask.Base
{
    public class Sitemap
    {
        public List<Uri> RootUris { get; private set; }
        public Dictionary<Uri,List<Uri>> Uris { get; private set; }

        public Sitemap()
        {
            Uris = new Dictionary<Uri, List<Uri>>();
            RootUris = new List<Uri>();
        }        
    }
}