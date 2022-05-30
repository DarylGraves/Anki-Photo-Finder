using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Api
{
    public abstract class Api
    {
        public string url;
        public HttpClient httpClient;

        public abstract Task<List<string>> QueryApiAsync(string keyWord);
        public abstract Task<List<Byte[]>> DownloadDataAsync(List<string> urls);
        public abstract void SaveData(byte[] data, string path, string filename);
        public async Task<List<Byte[]>> NextWordAsync(string word)
        {
            List<Byte[]> data = null;

            if (word is not null)
            {
                var results = await QueryApiAsync(word);
                data = await DownloadDataAsync(results);
            }
            else
            {
                throw new Exception("No keyword passed to Api.NextWord()");
            }

            return data;
        }
    }
}
