using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace HoujinBangouGetter
{
    public class BrowserClient : IDisposable
    {
        private const string BASE_URL = "https://www.houjin-bangou.nta.go.jp/";

        private IWebDriver webDriver;

        public BrowserClient()
        {
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            this.webDriver = new ChromeDriver(driverService);
        }

        public void Dispose()
        {
            this.webDriver?.Quit();
            this.webDriver?.Dispose();
            this.webDriver = null;
        }

        ~BrowserClient()
        {
            this.Dispose();
        }

        public async Task<IEnumerable<HojinNumber>> Search(string searchWord)
        {
            this.webDriver.Url = BASE_URL;

            // 適当な時間待機する
            Thread.Sleep(2000);
            this.AddSearchWord(searchWord);
            this.ClickSearchButton();

            // 適当な時間待機する
            Thread.Sleep(5000);
            return this.GetResult();
        }

        private void AddSearchWord(string searchWord)
        {
            var searchInput = this.webDriver.FindElement(By.Id("corp_name"));
            searchInput.SendKeys(searchWord.Replace("株式会社", string.Empty).Replace("有限会社", string.Empty));
        }

        private void ClickSearchButton()
        {
            var searchButton = this.webDriver.FindElement(By.Id("search_condition"));
            searchButton.Click();
        }


        private IEnumerable<HojinNumber> GetResult()
        {
            var hojinNumbers = new List<HojinNumber>();

            var resultTableRows = this.webDriver
                .FindElements(By.TagName("table"))
                .First()
                .FindElement(By.TagName("tbody"))
                .FindElements(By.TagName("tr"));

            foreach (var row in resultTableRows)
            {
                try
                {
                    var header = row.FindElement(By.TagName("th"));
                    var values = row.FindElements(By.TagName("td"));

                    // Add sample data
                    var number = header.Text;
                    var names = values[0].Text.Replace("\r\n", "\n").Split('\n');
                    var address = values[1].Text;
                    hojinNumbers.Add(new HojinNumber(
                        number,
                        names.Length > 1 ? names[1] : names[0],
                        names.Length > 1 ? names[0] : string.Empty,
                        address,
                        string.Empty)
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return hojinNumbers;
        }
    }
}
