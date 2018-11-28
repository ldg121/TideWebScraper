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
            GetTides("9418723");
            Console.ReadLine();
        }

        private static async void GetTides(string region_code)
        {
            //we need a url to grab tide info for
            var url = "https://tidesandcurrents.noaa.gov/noaatidepredictions.html?id="+ region_code;
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
            string[] data = new string[8];
            int counter = 0;
            foreach (var tide in tides)
            {
                var info = "";
                info = tide.InnerText;
                Console.WriteLine(info);
                string time = "";
                string height = "";
                //the next for loop is so that we can add every item we need an array
                foreach (var chara in info) 
                {
                    string temp = "";
                    if (chara != 'M')
                    {
                        //add to string time
                        temp = chara.ToString();
                        time = time + temp;
                    }
                    else if (chara == 'M')
                    {
                        //add 'm' to string time and string add to array 
                        temp = chara.ToString();
                        time = time + temp;
                        data[counter] = time;
                        counter++;
                    }
                    else if (chara == '-' /*|| Char.IsDigit(chara) == true ||
                             chara == 'f' || chara == 't'*/)
                    {
                        //add to string height
                        temp = chara.ToString();
                        height = height + temp;
                    }
                    else if (chara == '.')
                    {
                        //add to string height and array
                        temp = chara.ToString();
                        height = height + temp;
                        data[counter] = time;
                        counter++;
                        break;
                    }
                    else
                    {
                        //in between data
                        Console.WriteLine("I am in between the two data sets");

                    }
                }

            }

                Console.WriteLine();
            
            
        }
    }
}
