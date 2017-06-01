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
using WebGrease.Css.Extensions;

namespace UkadTestTask.Base
{
    public class SitemapProvider : ISitemapProvider
    {        
        public List<Sitemap> GetSitemapsFromUrl(string url)
        {
            List<Sitemap> result = new List<Sitemap>();

            XDocument xmlDocument = GetXmlDocument(url);

            if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "sitemapindex")
            {
                result.AddRange(
                    xmlDocument.Root
                        .Elements(XName.Get("sitemap", xmlDocument.Root.Name.NamespaceName))
                        .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                        .Select(el => new Sitemap(el.Value))
                );
                result = GetUrlsFromSitemaps(result);
            }
            else if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "urlset")
            {
                result.Add(new Sitemap(url, GetUrlsFromSitemap(url)));
            }
                
            return result;
        }

        private List<Sitemap> GetUrlsFromSitemaps(List<Sitemap> list)
        {            
            list.ForEach(r => r.Urls = GetUrlsFromSitemap(r.Url));            
            return list;
        }

        private List<SitemapUrl> GetUrlsFromSitemap(string sitemapUrl)
        {
            List<SitemapUrl> result = new List<SitemapUrl>();

            XDocument xmlDocument = GetXmlDocument(sitemapUrl);
            if (xmlDocument.Root == null)
                return result;

            if (xmlDocument.Root.Name.LocalName != "urlset")
                throw new Exception("Что-то пошло не так..");
            
            result.AddRange(xmlDocument.Root
                .Elements(XName.Get("url", xmlDocument.Root.Name.NamespaceName))
                .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                .Select(el => new SitemapUrl(el.Value)));
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