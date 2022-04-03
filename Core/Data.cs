using System.IO;
namespace Core;

public class Data
{
    public string[] csv { get; }
    public char delimiter { get; }
    
    // Constructor
    public Data(string path, char delimiter)
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
            // Then check it includes the delimiter 
            if (csvToValidate[0].Split(delimiter).Count() > 1)
            {
                // Accepted 
                this.csv = csvToValidate;
                this.delimiter = delimiter;
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
    public string[] ReturnHeaders()
    {
        return csv[0].Split(delimiter);
    }
}
