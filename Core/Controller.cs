using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Controller
    {
        public static Data? Data { get; set; }
        public static Api.Api? Api { get; set; }

        public static HttpClient httpClient = new HttpClient();

        public enum apiTypes
        {
            pexels
        }

        public static void LoadCsv(string path, char delimiter)
        {
            if (File.Exists(path))
            {
                Data = new Data(path, delimiter);
            }
            else
            { 
                throw new FileNotFoundException();
            }
        }
           
        public static void LoadApi(apiTypes type, string apiKey)
        {
            switch (type)
            {
                case apiTypes.pexels:
                    Api = new Api.Pexels(apiKey, httpClient);
                    break;
                default:
                    break;
            }
        }

    }
}
