using NUnit.Framework;
using System.IO;

using Core;

namespace nCore;

public class nData
{
    string csv_1_noData = "../../../../SharedFiles/Csvs/1_emptyFile.csv"; 
    string csv_2_OnlyHeaders = "../../../../SharedFiles/Csvs/2_onlyHeaders.csv"; 
    string csv_3_OneRowOfData = "../../../../SharedFiles/Csvs/3_oneRowOfData.csv";
    string csv_4_MultiRowsOfData = "../../../../SharedFiles/Csvs/4_MultiRowsOfData.csv";
    string saveLocation = "../../../../SharedFiles/Csvs/Output/Test.csv";

    [SetUp]
    public void Setup()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
        }
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
    public void CreateKeyWords()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
        
        var expectedOne = "Charlie";
        var expectedTwo = "Steve";
        var expectedThree = "Ally";
        var expectedFour = "Gonzo";

        StringAssert.AreEqualIgnoringCase(expectedOne, myData.Keywords[1]);
        StringAssert.AreEqualIgnoringCase(expectedTwo, myData.Keywords[2]);
        StringAssert.AreEqualIgnoringCase(expectedThree, myData.Keywords[3]);
        StringAssert.AreEqualIgnoringCase(expectedFour, myData.Keywords[4]);
    }
    
    [Test]
    public void CreateDictionaries_VerifyHeaders()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);

        var keyword = "name";
        var expectedOne = "age";
        var expectedTwo = "nationality";
        var expectedThree = "notes";

        StringAssert.AreEqualIgnoringCase(expectedOne, myData.WordRows[keyword][0]);
        StringAssert.AreEqualIgnoringCase(expectedTwo, myData.WordRows[keyword][1]);
        StringAssert.AreEqualIgnoringCase(expectedThree, myData.WordRows[keyword][2]);
    }
    
    [Test]
    public void CreateDictionaries_VerifyaRow_1()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
    
        var keyword = "Steve";
        var expectedOne = "45";
        var expectedTwo = "British";
        var expectedThree = "Works day and night";
    
        StringAssert.AreEqualIgnoringCase(expectedOne, myData.WordRows[keyword][0]);
        StringAssert.AreEqualIgnoringCase(expectedTwo, myData.WordRows[keyword][1]);
        StringAssert.AreEqualIgnoringCase(expectedThree, myData.WordRows[keyword][2]);
    }
    
    [Test]
    public void CreateDictionaries_VerifyaRow_2()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(3);
    
        var keyword = "A lawyer";
        var expectedOne = "Ally";
        var expectedTwo = "28";
        var expectedThree = "American";
    
        StringAssert.AreEqualIgnoringCase(expectedOne, myData.WordRows[keyword][0]);
        StringAssert.AreEqualIgnoringCase(expectedTwo, myData.WordRows[keyword][1]);
        StringAssert.AreEqualIgnoringCase(expectedThree, myData.WordRows[keyword][2]);
    }

    [Test]
    public void AddColumn()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);

        myData.AddColumn("HairColour");
        
        var result = myData.WordRows[myData.KeywordHeader].Contains("HairColour");
        Assert.IsTrue(result);

    }
    
    [Test]
    public void AddColumn_ReturnValue_1()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
    
        var result = myData.AddColumn("HairColour");
        Assert.AreEqual(3, result); 
    }

    [Test]
    public void AddNewColumn_ReturnValue_2()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
    
        var firstResult = myData.AddColumn("HairColour");
        var secondResult = myData.AddColumn("Height");

        Assert.AreEqual(3, firstResult); 
        Assert.AreEqual(4, secondResult);
    }

    [Test]
    public void AddColumnAndValue()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);

        var newColumn = myData.AddColumn("HairColour");
        var charlieColumn = myData.AddColumnValue("HairColour", "Charlie", "Brown");
        var steveColumn = myData.AddColumnValue("HairColour", "Steve", "Blonde");
        var allyColumn = myData.AddColumnValue("HairColour", "Ally", "Purple");   
        var gonzoColumn = myData.AddColumnValue("HairColour", "Gonzo", "N/A");   

        var charlieResult = myData.WordRows["Charlie"][charlieColumn];    
        var steveResult = myData.WordRows["Steve"][steveColumn];
        var allyResult = myData.WordRows["Ally"][allyColumn];
        var gonzoResult = myData.WordRows["Gonzo"][gonzoColumn];

        StringAssert.AreEqualIgnoringCase("Brown", charlieResult);
        StringAssert.AreEqualIgnoringCase("Blonde", steveResult);
        StringAssert.AreEqualIgnoringCase("Purple", allyResult);
        StringAssert.AreEqualIgnoringCase("N/A", gonzoResult);
    }

    [Test]
    public void AddValueNoColumn()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
    
        var charlieColumn = myData.AddColumnValue("HairColour", "Charlie", "Brown");
        var steveColumn = myData.AddColumnValue("HairColour", "Steve", "Blonde");
        var allyColumn = myData.AddColumnValue("HairColour", "Ally", "Purple");   
        var gonzoColumn = myData.AddColumnValue("HairColour", "Gonzo", "N/A");   
    
        var charlieResult = myData.WordRows["Charlie"][charlieColumn];    
        var steveResult = myData.WordRows["Steve"][steveColumn];
        var allyResult = myData.WordRows["Ally"][allyColumn];
        var gonzoResult = myData.WordRows["Gonzo"][gonzoColumn];
    
        StringAssert.AreEqualIgnoringCase("Brown", charlieResult);
        StringAssert.AreEqualIgnoringCase("Blonde", steveResult);
        StringAssert.AreEqualIgnoringCase("Purple", allyResult);
        StringAssert.AreEqualIgnoringCase("N/A", gonzoResult);
    }

    [Test]
    public void Save_1_NoChanges()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
        myData.Save(saveLocation);

        var myFile = File.ReadAllLines(saveLocation);

        string expectedHeader = "name,age,nationality,notes";
        string expectedSteve = "Steve,45,British,Works day and night";
        string expectedAlly = "Ally,28,American,A lawyer";

        StringAssert.AreEqualIgnoringCase(expectedHeader, myFile[0]);
        StringAssert.AreEqualIgnoringCase(expectedSteve, myFile[2]);
        StringAssert.AreEqualIgnoringCase(expectedAlly, myFile[3]);
    }

    [Test]
    public void Save_2_NewColumn()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);
        
        myData.AddColumn("HairColour");
        myData.AddColumnValue("HairColour", "Charlie", "Brown");
        myData.AddColumnValue("HairColour", "Steve", "Blonde");
        myData.AddColumnValue("HairColour", "Ally", "Purple");
        myData.AddColumnValue("HairColour", "Gonzo", "N/A");
        myData.Save(saveLocation);

        var MyFile = File.ReadAllLines(saveLocation);

        string expectedHeader = "name,age,nationality,notes,HairColour";
        string expectedSteve = "Steve,45,British,Works day and night,Blonde";
        string expectedAlly = "Ally,28,American,A lawyer,Purple";
        
        StringAssert.AreEqualIgnoringCase(expectedHeader, MyFile[0]);
        StringAssert.AreEqualIgnoringCase(expectedSteve, MyFile[2]);
        StringAssert.AreEqualIgnoringCase(expectedAlly, MyFile[3]);
    }

    [Test]
    public void AddColumn_IsWidthConsistentAcrossRows()
    {
        var myData = new Data(csv_4_MultiRowsOfData, ',');
        myData.CreateCollection(0);

        myData.AddColumn("HairColour");
        myData.Save(saveLocation);

        var myFile = File.ReadAllLines(saveLocation);

        string expectedHeader = "name,age,nationality,notes,HairColour";
        string expectedSteve = "Steve,45,British,Works day and night,NULL";
        string expectedAlly = "Ally,28,American,A lawyer,NULL";

        StringAssert.AreEqualIgnoringCase(expectedHeader, myFile[0]);
        StringAssert.AreEqualIgnoringCase(expectedSteve, myFile[2]);
        StringAssert.AreEqualIgnoringCase(expectedAlly, myFile[3]);
    }
}