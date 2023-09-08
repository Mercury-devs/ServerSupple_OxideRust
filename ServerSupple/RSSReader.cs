using System;
using System.Xml.Linq;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace ServerSupple
{
    internal class RSSReader
    {
        public static async Task RSSController()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = Path.Combine(currentDirectory, "latest_date.txt");

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://umod.org/games/rust.rss");
                if (response.IsSuccessStatusCode)
                {
                    string rssText = await response.Content.ReadAsStringAsync();
                    XDocument rssXml = XDocument.Parse(rssText);
                    XNamespace ns = "http://www.w3.org/2005/Atom";

                    var updatedElement = rssXml.Element(ns + "feed").Element(ns + "updated");
                    DateTime newDate = DateTime.Parse(updatedElement.Value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

                    DateTime oldDate;

                    if (File.Exists(logFilePath))
                    {
                        string oldDateString = File.ReadAllText(logFilePath);
                        oldDate = DateTime.Parse(oldDateString, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

                        if (newDate > oldDate)
                        {
                            File.WriteAllText(logFilePath, newDate.ToString("u"));
                            Console.WriteLine($"Обновляем версию Oxide, ожидайте");

                            await OxideDownloader.CheckOxide();
                        }
                        else
                        {
                            Console.WriteLine("Версия OXIDE актуальна, запускаем сервер");
                            Process.Start(Path.Combine(currentDirectory, "start.bat"));
                        }
                    }
                    else
                    {
                        File.WriteAllText(logFilePath, newDate.ToString("u"));
                        Console.WriteLine($"Обновляем версию Oxide, ожидайте");

                        await OxideDownloader.CheckOxide();
                    }
                }
                else
                {
                    Console.WriteLine($"Не удалось загрузить RSS. Код ошибки: {response.StatusCode}");
                }
            }
        }
    }
}
