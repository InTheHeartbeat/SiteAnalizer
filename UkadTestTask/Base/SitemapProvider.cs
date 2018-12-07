using SiteAnalyzer.Base.Interfaces;
using SiteAnalyzer.Scanning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiteAnalyzer.Base
{
    public class SitemapProvider : ISitemapProvider
    {
        public async Task<List<Sitemap>> GetSitemapsFromUrl(string url)
        {
            List<Sitemap> result = new List<Sitemap>();

            XDocument xmlDocument = await GetXmlDocument(url);
            if (xmlDocument == null)
            {
                return result;
            }

            if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "sitemapindex")
            {
                result.AddRange(
                    xmlDocument.Root
                        .Elements(XName.Get("sitemap", xmlDocument.Root.Name.NamespaceName))
                        .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                        .Select(el => new Sitemap(el.Value))
                );
                result = await GetUrlsFromSitemaps(result);
            }
            else if (xmlDocument.Root != null && xmlDocument.Root.Name.LocalName == "urlset")
            {
                result.Add(new Sitemap(url, await GetUrlsFromSitemap(url)));
            }

            return result;
        }

        private async Task<List<Sitemap>> GetUrlsFromSitemaps(List<Sitemap> list)
        {
            list.ForEach(async r => r.Urls = await GetUrlsFromSitemap(r.Url));
            return list;
        }

        private async Task<List<SitemapUrl>> GetUrlsFromSitemap(string sitemapUrl)
        {
            List<SitemapUrl> result = new List<SitemapUrl>();

            XDocument xmlDocument = await GetXmlDocument(sitemapUrl);
            if (xmlDocument?.Root == null)
                return result;

            if (xmlDocument.Root.Name.LocalName != "urlset")
                throw new Exception("Что-то пошло не так..");

            result.AddRange(xmlDocument.Root
                .Elements(XName.Get("url", xmlDocument.Root.Name.NamespaceName))
                .Elements(XName.Get("loc", xmlDocument.Root.Name.NamespaceName))
                .Select(el => new SitemapUrl(el.Value)));
            return result;
        }

        private async Task<XDocument> GetXmlDocument(string url)
        {
            try
            {
                return XDocument.Load(await LoadRawSitemapFromUrl(url));
            }
            catch
            {
                return null;
            }
        }

        private async Task<Stream> LoadRawSitemapFromUrl(string url)
        {
            WebResponse response = await WebRequest.Create(url).GetResponseAsync();
            if (response == null)
                throw new WebException("Response from url " + url + " is null.");
            return response.GetResponseStream();
        }
    }
}