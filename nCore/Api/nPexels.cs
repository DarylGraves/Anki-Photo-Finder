using NUnit.Framework;
using System.IO;
using Core.Api;
using System.Threading.Tasks;
using System.Text.Json;

namespace nCore;

public class nPexels
{
    string apiKey = "";
    
    [SetUp]
    public void Setup()
    {
        string keyPath = "../../../../SharedFiles/Secure/ApiKey_Pexels.txt"; 

        if (File.Exists(keyPath) != true)
        {
            throw new FileNotFoundException("Missing Pexels Api Key for Unit Tests");   
        }

        apiKey = File.ReadAllText(keyPath);
        apiKey = apiKey.Split('\n')[0];
    }

    [Test]
    public async Task DownloadPictures()
    {
        var pexels = new Pexels(apiKey, new System.Net.Http.HttpClient());
        var result = await pexels.QueryApiAsync("cat");

        // We can't assert the URL is correct in case the API changes it so we need to be more generic...

        int count = result.Count;

        //TEMP
        var pictures = await pexels.GetPicturesAsync(result);
        pexels.SavePictureAsync(pictures[0], "../../../../SharedFiles/Secure/PictureTest", "cat");
        Assert.Greater(count, 1);
    }

    [Test]
    public async Task SavePicture()
    {
        string filePath = "../../../../SharedFiles/Secure/PictureTest/";
        
        if (File.Exists(filePath + "cat.jpg"))
        {
            File.Delete(filePath + "cat.jpg");
        }

        var pexels = new Pexels(apiKey, new System.Net.Http.HttpClient());
        
        // This is NOT the usual way of doing this - usually we'd use pexels.GetPictureUrlAsync() but that calls the Api
        var resultAsText = File.ReadAllText("../../../../SharedFiles/JsonOutput/Pexels/cat.json");

        var resultAsJson = JsonSerializer.Deserialize<JsonResponse>(resultAsText);

        var urls = pexels.ConvertJsonToUrls(resultAsJson);

        var pics = await pexels.GetPicturesAsync(urls);
        pexels.SavePictureAsync(pics[0], filePath, "cat.jpg");
    }
}