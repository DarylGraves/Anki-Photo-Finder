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

    public async Task<List<string>> GetPictureUrls(string keyWord)
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

        List<string> links = new List<string>(); 

        for (int i = 0; i < jsonData.photos.Count(); i++)
        {
            links.Add(jsonData.photos[i].src.small);
        }

        return links;
    }
}