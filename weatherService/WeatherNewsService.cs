using System.Collections.Generic;
using System.Linq;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using weatherModel;

namespace weatherService
{
    public class WeatherNewsService : IWeatherNewsService
    {
        public WeatherNewsService()
        {
        }
            
        public IEnumerable<WeatherNews> GetWeatherNews()
        {
            return GetWeatherNewsUsingFizzler();
        }

        private IEnumerable<WeatherNews> GetWatherNeewsUsingAgility()
        {
            var htmlWeb = new HtmlWeb();
            var webPage = htmlWeb.Load("https://search.naver.com/search.naver?where=video&ie=utf8&sort=rel&listmode=v&stype=&period=&playtime=&selected_cp=&nso=&x_video=&sm=tab_pge&query=날씨&start=1&offset=100");
            var target = webPage.GetElementbyId("main_pack").Descendants("dt");

            var weatherNews = new List<WeatherNews> { };

            foreach (var item in target)
            {
                var thumbImage = item.ParentNode.PreviousSibling.PreviousSibling.
                    Descendants("img").Select(s => s.GetAttributeValue("src", null)).FirstOrDefault();
                var broadcastCompany = item.NextSibling.NextSibling.NextSibling.NextSibling
                    .Descendants("a").Select(s => s.GetAttributeValue("href", null)).FirstOrDefault();
                var title = item.Descendants("a").Select(s => s.GetAttributeValue("title", null)).FirstOrDefault();
                var movieLink = item.Descendants("a").Select(s => s.GetAttributeValue("href", null)).FirstOrDefault();

                weatherNews.Add(new WeatherNews
                {
                    Title = title,
                    LinkUrl = movieLink,
                    BroadcastingCompany = broadcastCompany,
                    Thumbnail = thumbImage,
                });
            }

            return weatherNews.AsEnumerable();
        }

        private IEnumerable<WeatherNews> GetWeatherNewsUsingFizzler()
        {
            var weatherNews = new List<WeatherNews> { };
            var htmlWeb = new HtmlWeb();

            var webPage = htmlWeb.Load("https://search.naver.com/search.naver?where=video&ie=utf8&sort=rel&listmode=v&stype=&period=&playtime=&selected_cp=&nso=&x_video=&sm=tab_pge&query=날씨&start=1&offset=100");
            var doc = new HtmlDocument();
            doc.LoadHtml(webPage.DocumentNode.OuterHtml);

            var e = doc.DocumentNode.QuerySelectorAll(".video_item");
            foreach(var item in e.Elements())
            {
                var title = item.QuerySelector(".info_title").Descendants("a").Select(x => x.GetAttributeValue("title", null)).FirstOrDefault();
                var movieLink = item.QuerySelector(".info_title").Descendants("a").Select(x => x.GetAttributeValue("href", null)).FirstOrDefault();
                var broadcastCompany = item.QuerySelector(".info.cite").Descendants("a").Select(x => x.GetAttributeValue("title", null)).FirstOrDefault();
                var thumbImage = item.QuerySelector(".video_thum").Descendants("img").Select(x => x.GetAttributeValue("src", null)).FirstOrDefault();
                weatherNews.Add(new WeatherNews
                {
                    Title = title,
                    LinkUrl = movieLink,
                    BroadcastingCompany = broadcastCompany,
                    Thumbnail = thumbImage,
                });
            }

            return weatherNews.AsEnumerable();
        }
    }
}
