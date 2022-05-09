using System.Text.Json;

namespace Core.Api;

public class Pexels
{
    
    private string url = "https://api.pexels.com/v1/";
    private HttpClient httpClient;
    public Pexels(string apiKey, HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(apiKey);
    }

    public async Task<List<string>> QueryApiAsync(string keyWord)
    {
        HttpResponseMessage response;
        try
        {
            string theUrl = url + "search?query=" + keyWord;
            response = await httpClient.GetAsync(theUrl);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException($"Error occured {e}");
        }

        // If debugging, comment out the below line and uncomment the other to avoid hammering the API
        var resultAsText = await response.Content.ReadAsStringAsync();
        //var resultAsText = File.ReadAllText("../../../../SharedFiles/JsonOutput/Pexels/cat.json");

        var jsonData = JsonSerializer.Deserialize<JsonResponse>(resultAsText);

        var links = ConvertJsonToUrls(jsonData);

        return links;
    }

    public List<string> ConvertJsonToUrls(JsonResponse json)
    {
        List<string> urls = new List<string>();

        for (int i = 0; i < json.photos.Count(); i++)
        {
            urls.Add(json.photos[i].src.small);
        }
        
        return urls;
    }

    public async Task<List<Byte[]>> GetPicturesAsync(List<String> urls)
    {
        List<Byte[]> pictures = new List<Byte[]>();

        foreach (var url in urls)
        {
            var response = await httpClient.GetAsync(url);
            
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (System.Exception e)
            {
                throw new HttpRequestException($"Error occured {e}");
            }

            byte[] picture = await response.Content.ReadAsByteArrayAsync();
            pictures.Add(picture);
        }

        return pictures;
    }

    public async void SavePictureAsync(byte[] data, string path, string filename)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await File.WriteAllBytesAsync(path + "/" + filename, data);
    }
}