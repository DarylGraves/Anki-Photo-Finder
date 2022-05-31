using System.Text.Json;

namespace Core
{
    public static class Controller
    {
        public static Data? Data { get; set; }
        public static Api.Api? Api { get; set; }

        public static List<Byte[]> PictureData { get; set; }
        public static string SavePicsLocation { get; set; }
        public static HttpClient httpClient = new HttpClient();
        public static event EventHandler OnNewPicturesAvailable;
        public static event EventHandler CallingApi;
        public static event EventHandler OnError;
        public static string ApiKey { get; set; }
        public static apiTypes SelectedApiType { get; set; }
        private static string SettingsLocation { get; set; }
        public static bool NewInstall { get; set; }
        public enum apiTypes
        {
            pexels
        }

        static Controller()
        {
            SavePicsLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\AnkiPhotoFinder";
            SettingsLocation = SavePicsLocation + "\\Settings";

            if (!Directory.Exists(SavePicsLocation)) Directory.CreateDirectory(SavePicsLocation);
            if (!Directory.Exists(SettingsLocation)) Directory.CreateDirectory(SettingsLocation);
            if (!File.Exists(SettingsLocation + "\\Settings.json"))
            {
                NewInstall = true;
            }
            else
            {
                NewInstall = false;
                LoadSettings();
            }
        }

        public static void UpdateApiType(string text)
        {
            Enum.TryParse<apiTypes>(text, out apiTypes type);
            SelectedApiType = type;
        }

        public static void LoadCsv(string path, char delimiter)
        {
            if (File.Exists(path))
            {
                try
                {
                    Data = new Data(path, delimiter);
                    Data.OnDataLoaded += NextPics;
                    LoadApi(SelectedApiType, ApiKey);
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e, EventArgs.Empty);
                }

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
            string SaveFile = (SavePicsLocation + "\\" + currentWord + ".jpg");
            File.WriteAllBytes(SaveFile, img);

            return SaveFile;
        }

        private static void LoadSettings()
        {
            var SettingsFile = SettingsLocation + "\\Settings.json";
            if (File.Exists(SettingsFile))
            {
                var file = File.ReadAllText(SettingsFile);
                SettingsJson settings = (SettingsJson)JsonSerializer.Deserialize(file, typeof(SettingsJson));

                Enum.TryParse<apiTypes>(settings.ApiType, out apiTypes result);
                SelectedApiType = result;

                ApiKey = settings.ApiKey;
            }
        }

        public static void SaveSettings()
        {
            var SettingsFile = SettingsLocation + "\\Settings.json";

            SettingsJson settingsToSave = new SettingsJson();
            settingsToSave.ApiType = SelectedApiType.ToString();
            settingsToSave.ApiKey = ApiKey;
            string json = JsonSerializer.Serialize(settingsToSave);
            File.WriteAllText(SettingsFile, json);
        }
    }
}
