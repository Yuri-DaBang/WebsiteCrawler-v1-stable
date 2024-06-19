using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

class LinkCrawler
{
    private HashSet<string> visitedLinks;

    public LinkCrawler()
    {
        visitedLinks = new HashSet<string>();
    }

    public void Crawl(string url, int maxDepth)
    {
        CrawlInternal(url, 0, maxDepth);
    }

    private void CrawlInternal(string url, int depth, int maxDepth)
    {
        try
        {
            if (depth > maxDepth || visitedLinks.Contains(url))
                return;

            visitedLinks.Add(url);

            if (url.Contains("https://"))
            {
                if (url.Contains(".php"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[php]");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[HTTPS]");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
            }
            else if (url.Contains("http://"))
            {
                if (url.Contains(".php"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[php]");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[HTTP]");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
            }
            else
            {
                if (url.Contains(".php"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[php]");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\n[+] ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[" + depth + "] ");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" Visiting: " + url);
                }
            }

            WebClient webClient = new WebClient();
            string pageContent = webClient.DownloadString(url);

            List<string> links = ExtractLinks(pageContent, "a", "href");
            List<string> cssLinks = ExtractLinks(pageContent, "link", "href");
            List<string> jsLinks = ExtractLinks(pageContent, "script", "src");
            List<string> codeLinks = FindCodeFiles(pageContent);

            foreach (string link in links)
            {
                CrawlInternal(link, depth + 1, maxDepth);
            }

            PrintLinks(cssLinks, "CSS", depth);
            PrintLinks(jsLinks, "JavaScript", depth);
            PrintLinks(codeLinks, "Code", depth);

        }
        catch
        {
            if (depth > maxDepth || visitedLinks.Contains(url))
                return;

            visitedLinks.Add(url);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n[+] ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[" + depth + "] ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[!] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("VISITED BUT NOT FOUND MORE LINKS: " + url);
        }
    }

    private List<string> ExtractLinks(string pageContent, string tag, string attributeName)
    {
        List<string> links = new List<string>();
        string startTag = "<" + tag;
        string endTag = "</" + tag + ">";
        int startIndex = 0;

        while (true)
        {
            startIndex = pageContent.IndexOf(startTag, startIndex);
            if (startIndex == -1)
                break;

            int endIndex = pageContent.IndexOf(">", startIndex);
            if (endIndex == -1)
                break;

            string tagContent = pageContent.Substring(startIndex, endIndex - startIndex + 1);

            int attributeStartIndex = tagContent.IndexOf(attributeName + "=\"");
            if (attributeStartIndex != -1)
            {
                attributeStartIndex += attributeName.Length + 2;
                int attributeEndIndex = tagContent.IndexOf("\"", attributeStartIndex);
                if (attributeEndIndex != -1)
                {
                    string link = tagContent.Substring(attributeStartIndex, attributeEndIndex - attributeStartIndex);
                    links.Add(link);
                }
            }

            startIndex = endIndex + 1;
        }

        return links;
    }

    private List<string> FindCodeFiles(string pageContent)
    {
        List<string> codeFiles = new List<string>();

        // Find PHP files
        MatchCollection phpMatches = Regex.Matches(pageContent, @"<a[^>]+href=[""']([^""']+\.(?:php))[""']", RegexOptions.IgnoreCase);
        foreach (Match match in phpMatches)
        {
            string link = match.Groups[1].Value;
            codeFiles.Add(link);
        }

        // Add more code file types and corresponding regex patterns as needed

        return codeFiles;
    }

    private void PrintLinks(List<string> links, string linkType, int depth)
    {
        foreach (string link in links)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n[+] ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[" + depth + "] ");

            if (linkType.Equals("CSS"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("[" + linkType + "] ");
            }
            else if (linkType.Equals("JavaScript"))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("[" + linkType + "] ");
            }
            else if (linkType.Equals("Code"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[" + linkType + "] ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("[" + linkType + "] ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Link: " + link);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            // Create an instance of the LinkCrawler class
            LinkCrawler crawler = new LinkCrawler();

            Console.Write("Enter the URL of the target website: ");
            string startingUrl = Console.ReadLine();

            //Console.Write("Enter the --max-depth : ");
            int maxDepth = 999999;

            //Console.Write("Enter the MAX DEPTH of the crawling process: ");
            //int hahahahahadepthahahahahahaha = Console.Read();

            // Specify the starting URL and maximum depth
            //int maxDepth = 9999999;

            // Start crawling
            crawler.Crawl(startingUrl, maxDepth);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n[!] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Crawling completed.");
        }
    }
}