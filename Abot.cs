using Abot2.Crawler;
using Abot2.Poco;
using AbotX2.Crawler;
using AbotX2.Poco;
using Parsing;
using System;
using System.Net;

namespace Abot
{
   public class AbotC
    {
        private static CrawlConfigurationX GetSafeConfig()
        {
            /*The following settings will help not get your ip banned
             by the sites you are trying to crawl. The idea is to crawl
             only 5 pages and wait 2 seconds between http requests
             */
            return new CrawlConfigurationX
            {
                MaxPagesToCrawl = 10000,
                MinCrawlDelayPerDomainMilliSeconds = 10,
            };
        }
        public async void Start(string uri)
        {
            var crawler = new CrawlerX(GetSafeConfig());
            crawler.PageCrawlStarting += Crawler_PageCrawlStarting;
            crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;
            crawler.PageCrawlDisallowed += Crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowed += Crawler_PageLinksCrawlDisallowed;


            var siteToCrawl = new Uri(uri);
            //var result = crawler.Crawl(new Uri("YourSiteHere"));
            var crawlerTask = crawler.CrawlAsync(siteToCrawl);

            //System.Threading.Thread.Sleep(30000);
            //crawler.Stop();
            //System.Threading.Thread.Sleep(10000);
            //crawler.Resume();

            var result = crawlerTask.Result;
        }

        private static void Crawler_PageLinksCrawlDisallowed(object sender, Abot2.Crawler.PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Crawler_PageCrawlDisallowed(object sender, Abot2.Crawler.PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine(crawledPage.Content.Text); // HTML

        }
        private static void Crawler_PageCrawlCompleted(object sender, Abot2.Crawler.PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            string boof = crawledPage.Content.Text;

            if (crawledPage.HttpRequestException != null || crawledPage.HttpResponseMessage.StatusCode != HttpStatusCode.OK)
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            else
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
                Console.WriteLine("Page had no content {0}", crawledPage.Uri.AbsoluteUri);


            ///PageInRowHTML - curent HTML page
            string PageInRowHTML = crawledPage.Content.Text;
            ///Transfer page for Parsing and Data copasitiv
            Parse operation = new Parse();
            operation.OperatePage(PageInRowHTML, crawledPage.Uri.AbsoluteUri);
        }

        private static void Crawler_PageCrawlStarting(object sender, Abot2.Crawler.PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            var crawler = new CrawlerX(GetSafeConfig());
            //ar context = e.CrawlContext;
            //var crawlerTask = crawler.CrawlAsync(pageToCrawl.Uri.AbsoluteUri);
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }
    }
}
