﻿using System;
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

        public static List<Byte[]> PictureData { get; set; }
        public static string SaveLocation { get; set; }
        public static HttpClient httpClient = new HttpClient();
        public static event EventHandler OnNewPicturesAvailable;
        public static event EventHandler CallingApi;

        public enum apiTypes
        {
            pexels
        }

        static Controller()
        {
            SaveLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AnkiPhotoFinder";

            if (!Directory.Exists(SaveLocation))
            {
                Directory.CreateDirectory(SaveLocation);
            }
        }

        public static void LoadCsv(string path, char delimiter)
        {
            if (File.Exists(path))
            {
                Data = new Data(path, delimiter);
                Data.OnDataLoaded += NextPics;

                //TODO: Controller.LoadCsv(): This shouldn't really be here but should be configured along with the settings...
                //TODO: Controller.LoadCsv(): Api key should be stored in settings and not pulled from a text file...
                LoadApi(apiTypes.pexels, File.ReadAllText(@"C:\Dev\Anki-Photo-Finder\SharedFiles\Secure\PexelsApi.txt"));
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
        
        public static async void NextPics(object sender, EventArgs e)
        {
            if (Data.KeywordsToDo.Count > 0)
            {
                CallingApi?.Invoke(null, EventArgs.Empty);

                var currentKeyword = Data.KeywordsToDo.Peek();
                var PicturesUrls = await Api.QueryApiAsync(currentKeyword);

                PictureData = await Api.DownloadDataAsync(PicturesUrls);
                OnNewPicturesAvailable?.Invoke(null, EventArgs.Empty);
            }
        }

        public static string SavePic(string currentWord, byte[] img)
        {
            string SaveFile = (SaveLocation + "\\" + currentWord + ".jpg");
            File.WriteAllBytes(SaveFile, img);

            return SaveFile;
        }
    }
}
