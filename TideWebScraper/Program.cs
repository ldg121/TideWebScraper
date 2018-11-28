using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;

namespace TideWebScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetTides();
            Console.ReadLine();
        }

        private static async void GetTides()
        {
            //we need a url to grab tide info for
            var url = "https://tidesandcurrents.noaa.gov/noaatidepredictions.html?id=9418723";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            //creates new html document
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var Tides_html = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("span3 well")).ToList();
            //grab predictions table and put every line into a data structure to get every 
            //value in that place

            var tides_table = Tides_html[0].Descendants("table")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("table table-condensed")).ToList();

            var tides = tides_table[0].Descendants("tr")
                .Where(node => node.GetAttributeValue("class","")
                .Equals("")).ToList();

            int high_tide = 0;
            int low_tide = 0;

            foreach (var tide in tides)
            {
                var info = "";
                info = tide.InnerText;
                Console.WriteLine(info);

                if (info[7] == 'l')
                {
                    Console.WriteLine("this is a low tide");
                    Console.WriteLine(low_tide);
                    low_tide++;
                }
                    
                else
                {
                    Console.WriteLine("this is a high tide");
                    Console.WriteLine(high_tide);
                    high_tide++;
                }
                    
            }

                Console.WriteLine();
            
            
        }
    }
}
