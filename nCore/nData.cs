using NUnit.Framework;
using Core;

namespace nCore;

public class Tests
{
    string csv_1_noData = "../../../../Debug/Csvs/1_emptyFile.csv"; 
    string csv_2_OnlyHeaders = "../../../../Debug/Csvs/2_onlyHeaders.csv"; 
    string csv_3_OneRowOfData = "../../../../Debug/Csvs/3_oneRowOfData.csv";
    
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
    public void ctor_3_OneRowOfData_ValidDelimiter()
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
}