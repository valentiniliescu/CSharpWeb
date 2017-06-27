using CSharpWeb;
using CSharpWeb.Runtime.Interop;
using System.Net.Http;

namespace CSharpWebApplication1
{
    public class Program
    {
        static void Main()
        {
            var rootDiv = new HTMLDivElement();
            HTMLElement.AppendChildToRoot(rootDiv);

            var div = new HTMLDivElement();
            rootDiv.AppendChild(div);
            div.InnerText = "Hello world!";

            var button1 = new HTMLButtonElement();
            rootDiv.AppendChild(button1);
            button1.InnerText = "Click me!";

            button1.Click += () => { div.InnerText = "Hello world from button!"; };

            var button2 = new HTMLButtonElement();
            rootDiv.AppendChild(button2);
            button2.InnerText = "Load data!";

            button2.Click += async () => {
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync("weather.json");
                    var weatherForecastArray = JsonUtil.Deserialize<WeatherForecast[]>(json);
                    div.InnerText = weatherForecastArray[0].ToString();
                }
            };
        }
    }

    public class WeatherForecast
    {
        public string DateFormatted { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }

        public override string ToString()
        {
            return $"Date: {DateFormatted} Temperature: {TemperatureC} Summary: {Summary}";
        }
    }
}
