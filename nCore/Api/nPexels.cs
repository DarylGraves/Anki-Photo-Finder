using NUnit.Framework;
using System.IO;
using Core.Api;

namespace nCore;

public class nPexels
{
    [SetUp]
    public void Setup()
    {
        string keyPath = "../../../../SharedFiles/Secure/ApiKey_Pexels.txt"; 

        if (File.Exists(keyPath) != true)
        {
            throw new FileNotFoundException("Missing Pexels Api Key for Unit Tests");   
        }

        string apiKey = File.ReadAllText(keyPath);
    }

    [Test]
    public void Template()
    {
        var Pexels = new Pexels("Hello, World");
        StringAssert.AreEqualIgnoringCase("Hello, World", Pexels.Test);
    }
}