using NUnit.Framework;
using System.IO;
using Core.Api;
using System.Threading.Tasks;

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
    public async Task Template()
    {
        var pexels = new Pexels(apiKey, new System.Net.Http.HttpClient());
        var result = await pexels.GetPictureUrls("cat");

        // We can't assert the URL is correct in case the API changes it so we need to be more generic...

        int count = result.Count;

        Assert.Greater(count, 1);
    }
}