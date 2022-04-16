using System.IO;
namespace Core;

public class Data
{
    //TODO: Change most of these to Read-Only and use private variables for changes inside the class
    public string[] Csv { get; }
    public char Delimiter { get; }
    public string[] Headers { get; }
    public string? KeywordHeader { get; set; }
    public List<string> Keywords {get; set;}
    public Dictionary<string, List<string>> WordRows { get; set; } 
    public Stack<string>? KeywordsToDo { get; set; }
    public Stack<string>? KeywordsComplete { get; set; }

    // Constructor
    public Data(string path, char Delimiter)
    {
        string[] csvToValidate;
        
        // Load CSV
        if (File.Exists(path))
        {
            csvToValidate = File.ReadAllLines(path);               
        }
        else
        {
            throw new FileNotFoundException($"{path} not found!");
        }

        // Validate CSV is valid
        
        // First check there is more than just a header
        if (csvToValidate.Length > 1)
        {
            // Then check it includes the Delimiter 
            if (csvToValidate[0].Split(Delimiter).Count() > 1)
            {
                // This File was accepted 
                this.Csv = csvToValidate;
                this.Delimiter = Delimiter;


                this.Keywords = new List<string>();
                this.Headers = csvToValidate[0].Split(Delimiter);
                this.WordRows = new Dictionary<string, List<string>>();
            }
            else
            {
                throw new Exception("Invalid Csv File - Delimiter was not found");
            }
        }
        else
        {
            throw new Exception("Invalid Csv File - File was less than 2 lines");
        }
    }

    ///<summary>
    /// Returns all the heads of the loaded Csv as a string array.
    ///</summary>
    public string[] GetHeaders()
    {
        return Headers;
    }

    public int FindColumnNo(string headerToFind)
    {
        for (int i = 0; i < Headers.Length; i++)
        {
            if (Headers[i] == headerToFind)
            {
                return i;
            }
        }
        
        // Header doesn't exist
        return -1;
    }

    public void CreateCollection(int columnNo)
    {
        if (Csv == null)
        {
           return;
        }

        KeywordHeader = Csv[0].Split(Delimiter)[columnNo];

        foreach (var row in Csv)
        {
            var keyword = row.Split(Delimiter)[columnNo];
            Keywords.Add(keyword);

            List<string> elements = new List<string>();
            foreach (var element in row.Split(Delimiter))
            {
                if (element != keyword)
                {
                    elements.Add(element);
                }
            }

            WordRows.Add(keyword, elements);
        }
    }

    public int AddColumn(string columnName)
    {
        if(KeywordHeader == null)
        {
            // CreateCollection() hasn't been ran yet
            return -1;
        }

        // Check if header already exists
        if(!(WordRows.ContainsKey(columnName)) && (!(WordRows[KeywordHeader].Contains(columnName))))
        {
            WordRows[KeywordHeader].Add(columnName);

            for (int i = 0; i < WordRows[KeywordHeader].Count; i++)
            {
                if (WordRows[KeywordHeader][i] == columnName)
                {
                    return i;
                }
            }
        }
        return -1;
    }
   
    public int AddColumnValue(string columnName, string keyword, string value)
    {
        //TODO: AddColumNValue()
        return 0;
    } 
    //TODO: Save()
        // Saves the Collection down as a CSV ready for Anki
}