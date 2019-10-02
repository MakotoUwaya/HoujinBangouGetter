using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoujinBangouGetter
{
    public class HojinNumber
    {
        public string 法人番号 { get; }
        public string 商号又は名称 { get; }
        public string 名称カナ { get; }
        public string 所在地 { get; }
        public Uri 変更履歴情報等 { get; }

        public HojinNumber(string hojinNumber, string name, string nameKana, string address, string historyUrl)
        {
            this.法人番号 = hojinNumber;
            this.商号又は名称 = name;
            this.名称カナ = nameKana;
            this.所在地 = address;
            if (Uri.TryCreate(historyUrl, UriKind.Absolute, out var url))
            {
                this.変更履歴情報等 = url;
            }
        }

        public static async Task<List<HojinNumber>> Search(string[] campanyNames)
        {
            var hojinNumbers = new List<HojinNumber>();
            using (var client = new BrowserClient())
            {
                foreach (var companyName in campanyNames)
                {
                    hojinNumbers.AddRange(await client.Search(companyName));
                }
                return hojinNumbers;
            }
        }
    }
}
