using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using UkadTestTask.Base.Interfaces;
using UkadTestTask.Scanning;

namespace UkadTestTask.Base
{
    public class SitemapProvider : ISitemapProvider
    {
        private ScannableUrl _requestUri = null;

        public Sitemap GetSitemapFromUrl(string url)
        {
            _requestUri = new ScannableUrl(url);
            return CreateFromXml(GetXmlDocument(url));
        }

        private Sitemap CreateFromXml(XDocument xmlDocument)
        {
            Sitemap result = new Sitemap();

            if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "sitemapindex")
            {
                result.RootUris.AddRange(
                    xmlDocument.Root
                        .Elements(XName.Get("sitemap", xmlDocument.Root.Name.NamespaceName))
                        .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                        .Select(el => new ScannableUrl(el.Value))
                );

                foreach (ScannableUrl rootUri in result.RootUris)
                {
                    XDocument subDoc = GetXmlDocument(rootUri.Url);
                    if (subDoc.Root == null)
                        break;

                    if (subDoc.Root.Name.LocalName != "urlset")
                        throw new Exception("Что-то пошло не так..");

                    List<ScannableUrl> temp = new List<ScannableUrl>();
                    temp.AddRange(subDoc.Root
                        .Elements(XName.Get("url", subDoc.Root.Name.NamespaceName))
                        .Elements(XName.Get("loc", subDoc.Root.Name.NamespaceName))
                        .Select(el => new ScannableUrl(el.Value)));

                    result.Uris.Add(rootUri, temp);
                }
            }
            else if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "urlset")
            {
                List<ScannableUrl> temp = new List<ScannableUrl>();
                temp.AddRange(xmlDocument.Root
                    .Elements(XName.Get("url", xmlDocument.Root.Name.NamespaceName))
                    .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                    .Select(el => new ScannableUrl(el.Value)));
                
                result.Uris.Add(_requestUri, temp);
                result.RootUris.Add(_requestUri);
            }
            return result;
        }

        private XDocument GetXmlDocument(string url)
        {
            return XDocument.Load(LoadRawSitemapFromUrl(url));                        
        }

        private Stream LoadRawSitemapFromUrl(string url)
        {
            HttpWebResponse response = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(url)).GetResponse();
            if(response == null)
                throw new WebException("Response from url " + url + " is null.");
            return response.GetResponseStream();                                      
        }
    }
}