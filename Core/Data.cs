using System.IO;
using System.Text;

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
        //TODO: Data.AddColumn() - Should add null entries manually for every row
        if(KeywordHeader == null)
        {
            // CreateCollection() hasn't been ran yet
            return -1;
        }

        // If the header doesn't already exist
        if(!(WordRows.ContainsKey(columnName)) && (!(WordRows[KeywordHeader].Contains(columnName))))
        {
            WordRows[KeywordHeader].Add(columnName);

            // Add a new cell in each row to ensure they all keep the same width
            foreach (var keyword in Keywords)
            {
                // Skip the header row
                if (keyword != KeywordHeader)
                {
                    WordRows[keyword].Add("NULL");       
                }
            }


            // Find the column number of the new entry and return it
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
        
        // If the column doesn't already exist, create it
        if (columnNo == -1)
        {
            columnNo = AddColumn(columnName);
        }

        // Now add the value to the column
        WordRows[keyword][columnNo] = value;
        return columnNo;     
    } 

    public void Save(string path)
    {
        //TODO: Data.Save() Need to make sure this is still working correctly when we add a new column and data to the column. What happens if we have rows which vary in column length?
        //TODO: Data.Save() What happens if a cell has a space - Do we need to wrap it in quotes?
        //TODO: Data.Save() What happens if a cell has quotation marks - Do we need to do anything special?
        //TODO: Data.Save() What happens if a cell has a space AND quotation marks? Need to look into how Csvs work in this respect.

        var destinationFile = File.AppendText(path);
        try
        {
            foreach (var keyword in Keywords)
            {
                StringBuilder rows = new StringBuilder();

                for (int i = 0; i < WordRows[keyword].Count; i++)
                {
                    if (i != WordRows[keyword].Count -1)
                    {
                        rows.Append(WordRows[keyword][i] + Delimiter);
                    }
                    else
                    {
                        rows.Append(WordRows[keyword][i]);
                    }
                }

                string rowAsString = keyword + Delimiter + rows + '\n';
                destinationFile.Write(rowAsString);
            }          

            destinationFile.Close();
        }
        catch (System.Exception)
        {
            destinationFile.Close();
            throw;
        }
    }
}