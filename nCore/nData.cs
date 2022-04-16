using NUnit.Framework;
using Core;

namespace nCore;

public class nData
{
    string csv_1_noData = "../../../../SharedFiles/Csvs/1_emptyFile.csv"; 
    string csv_2_OnlyHeaders = "../../../../SharedFiles/Csvs/2_onlyHeaders.csv"; 
    string csv_3_OneRowOfData = "../../../../SharedFiles/Csvs/3_oneRowOfData.csv";
    string csv_4_MultiRowsOfData = "../../../../SharedFiles/Csvs/4_MultiRowsOfData.csv";
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ctor_1_EmptyFile()
    {
        bool dataNotLoaded = true;
        try
        {
            var myData = new Data(csv_1_noData, ',');
            dataNotLoaded = true;    
        }
        catch {}
        
        Assert.IsTrue(dataNotLoaded);
    }
    
    [Test]
    public void ctor_2_OnlyHeaders()
    {
        bool dataNotLoaded = true;
        try
        {
            var myData = new Data(csv_2_OnlyHeaders, ',');
            dataNotLoaded = true;    
        }
        catch {}
        
        Assert.IsTrue(dataNotLoaded);
    }

    [Test]
    public void ctor_3_OneRowOfData_InvalidDelimiter()
    {
        bool dataNotLoaded = true;
        try
        {
            var myData = new Data(csv_3_OneRowOfData, ';');
            dataNotLoaded = true;    
        }
        catch {}
        
        Assert.IsTrue(dataNotLoaded);
    }

    [Test]
    public void ctor_3_OneRowOfData()
    {
        bool dataLoaded = false;
        try
        {
            var myData = new Data(csv_3_OneRowOfData, ',');
            dataLoaded = true;    
        }
        catch {}
        
        Assert.IsTrue(dataLoaded);
    }

    [Test]
    public void GetHeaders()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.GetHeaders();

        StringAssert.AreEqualIgnoringCase("name", result[0]);
        StringAssert.AreEqualIgnoringCase("age", result[1]);
        StringAssert.AreEqualIgnoringCase("nationality", result[2]);
        StringAssert.AreEqualIgnoringCase("notes", result[3]);
    }
    
    [Test]
    public void FindColumnNo_1()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.FindColumnNo("name");

        Assert.AreEqual(0, result);
    }

    [Test]
    public void FindColumnNo_2()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.FindColumnNo("age");

        Assert.AreEqual(1, result);
    }

    [Test]
    public void FindColumnNo_3()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.FindColumnNo("nationality");

        Assert.AreEqual(2, result);
    }

    [Test]
    public void FindColumnNo_4()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.FindColumnNo("notes");

        Assert.AreEqual(3, result);
    }
    
    [Test]
    public void FindColumnNo_X()
    {
        var myData = new Data(csv_3_OneRowOfData, ',');
        var result = myData.FindColumnNo("This doesn't exist!");

        Assert.AreEqual(-1, result);
    }

    [Test]
    public void CreateListOfWords()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
        
        var expectedOne = "Charlie";
        var expectedTwo = "Steve";
        var expectedThree = "Ally";
        var expectedFour = "Gonzo";

        StringAssert.AreEqualIgnoringCase(expectedOne, myData.Words[0]);
        StringAssert.AreEqualIgnoringCase(expectedTwo, myData.Words[1]);
        StringAssert.AreEqualIgnoringCase(expectedThree, myData.Words[2]);
        StringAssert.AreEqualIgnoringCase(expectedFour, myData.Words[3]);
    }
    
}