using NUnit.Framework;
using Core;

namespace nCore;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var myData = new Data("Hello, world!");
        Assert.AreEqual("Hello, world", myData.myGreeting);
        Assert.Pass();
    }
}