using System.IO;
namespace Core;

public class Data
{
    //TODO: Change most of these to Read-Only and use private variables for changes inside the class
    private string[] csv;
    private char Delimiter;
    private string[] Headers; 
    private Stack<string> KeywordsToDo;
    private Stack<string> KeywordsCompleted;
    
    public Dictionary<string, List<string>> WordRows { get; private set; } 
    public List<string> Keywords {get; private set;}
    public string? KeywordHeader { get; private set; }

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
            // TODO: Are we checking that each row has the same number of columns?
            // Then check it includes the Delimiter 
            if (csvToValidate[0].Split(Delimiter).Count() > 1)
            {
                // This File was accepted 
                this.csv = csvToValidate;
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

    ///<summary>
    /// Searches through the Csv headers for the "headerToFind" string. If found, returns the matching column number.
    ///</summary>
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

    ///<summary>
    /// Used to create the Dictionary variable of the Csv. This requires a column number from the Csv first to determine index.
    ///</summary>
    public void CreateCollection(int columnNo)
    {
        //TODO: CreateCollection() we could make an override which takes the KeywordHeader directly so no faffing around with column numbers.

        if (csv == null)
        {
           return;
        }

        // Get the Header 
        KeywordHeader = csv[0].Split(Delimiter)[columnNo];

        // Construct the Dictionary
        foreach (var row in csv)
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

        // Create the Stacks we'll be working through
        KeywordsToDo = new Stack<string>();
        KeywordsCompleted = new Stack<string>();
    }

    ///<summary>
    /// Adds a column header to the dictionary
    ///</summary>
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
   
    ///<summary>
    /// Adds a value into a cell at the coordinates of the columnName (y) and keyword (x)
    ///</summary>
    public int AddColumnValue(string columnName, string keyword, string value)
    {
        // TODO: AddColumnValue()
        if (KeywordHeader == null)
        {
            //CreateCollection() hasn't been ran yet
            return -1;
        }

        // Get the column number
        var columnNo = -1;
        for (int i = 0; i < WordRows[KeywordHeader].Count; i++)
        {
            if (WordRows[KeywordHeader][i] == columnName)
            {
                columnNo = i;
            }
        }
        
        if (columnNo == -1)
        {
            columnNo = AddColumn(columnName);
        }

        // Now add the value to that column
        try
        {
            WordRows[keyword][columnNo] = value;     
        }
        catch (System.Exception)
        {
            // The List<string> was not already present so needs to be made
            var noOfColumns = WordRows[keyword].Count;
            
            if (noOfColumns < columnNo)
            {
                // TODO: AddColumnValue() - Might be better to have it update EVERY List<string> to make sure they're all consistently the same size.

                // Add the required number of columns to the row.
                var requiredColumns = columnNo - noOfColumns;
                for (int i = 0; i < (requiredColumns -1); i++)
                {
                    WordRows[keyword].Add("NULL");
                }

            }
            
            WordRows[keyword].Add(value);
            return columnNo;
        }
        
        return -1;
    } 
    //TODO: Save()
        // Saves the Collection down as a CSV ready for Anki
}